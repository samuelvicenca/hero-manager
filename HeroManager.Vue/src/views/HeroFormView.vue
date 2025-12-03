<template>
  <div class="page">
    <div class="page__card">
      <header class="page__header">
        <div>
          <h1 class="page__title">
            {{ isEdit ? 'Editar Herói' : 'Novo Herói' }}
          </h1>
          <p class="page__subtitle">
            Preencha os dados abaixo para
            {{ isEdit ? 'atualizar' : 'cadastrar' }} um herói.
          </p>
        </div>

        <button type="button" class="btn btn--ghost" @click="goBack">
          Voltar
        </button>
      </header>

      <section v-if="loading" class="page__status">
        Carregando...
      </section>

      <section v-else>
        <form class="form" @submit.prevent="onSubmit">
          <div class="form__group">
            <label class="form__label" for="nome">Nome</label>
            <input
              id="nome"
              v-model="form.nome"
              type="text"
              class="form__input"
              required
              placeholder="Ex: Peter Parker"
            />
          </div>

          <div class="form__group">
            <label class="form__label" for="nomeHeroi">Nome de Herói</label>
            <input
              id="nomeHeroi"
              v-model="form.nomeHeroi"
              type="text"
              class="form__input"
              required
              placeholder="Ex: Homem-Aranha"
            />
          </div>

          <div class="form__group">
            <label class="form__label" for="dataNascimento">Data de Nascimento</label>
            <input
              id="dataNascimento"
              v-model="form.dataNascimento"
              type="date"
              class="form__input"
              required
            />
          </div>

          <div class="form__row">
            <div class="form__group">
              <label class="form__label" for="altura">Altura (m)</label>
              <input
                id="altura"
                v-model.number="form.altura"
                type="number"
                step="0.01"
                min="0"
                class="form__input"
                required
              />
            </div>

            <div class="form__group">
              <label class="form__label" for="peso">Peso (kg)</label>
              <input
                id="peso"
                v-model.number="form.peso"
                type="number"
                step="0.1"
                min="0"
                class="form__input"
                required
              />
            </div>
          </div>

          <div class="form__group">
            <label class="form__label">Superpoderes</label>

            <div v-if="superpowersLoading" class="form__hint">
              Carregando superpoderes...
            </div>

            <div v-else>
              <div v-if="superpowersError" class="form__hint form__hint--error">
                {{ superpowersError }}
              </div>

              <div v-if="superpowers.length" class="superpowers-grid">
                <label
                  v-for="sp in superpowers"
                  :key="sp.id"
                  class="superpower-pill"
                >
                  <input
                    type="checkbox"
                    :value="sp.id"
                    v-model="form.superpoderIds"
                  />
                  <span class="superpower-pill__name">{{ sp.superpoder }}</span>
                  <span class="superpower-pill__desc">{{ sp.descricao }}</span>
                </label>
              </div>

              <div v-else class="empty-superpowers">
                <p class="form__hint">
                  Nenhum superpoder disponível no momento.
                </p>
                <button
                  type="button"
                  class="btn btn--primary btn--sm"
                  @click="goToCreateSuperpower"
                >
                  Cadastrar superpoder
                </button>
              </div>
            </div>
          </div>

          <div class="form__actions">
            <button
              type="submit"
              class="btn btn--primary"
              :disabled="saving"
            >
              {{ isEdit ? 'Salvar Alterações' : 'Cadastrar Herói' }}
            </button>
          </div>
        </form>
      </section>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted, ref, watch } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import type { HeroInput } from '../types/hero';
import { getHero, createHero, updateHero } from '../api/heroes';

import type { Superpower } from '../types/superpower';
import { getSuperpowers } from '../api/superpowers';

const route = useRoute();
const router = useRouter();

const FORM_STORAGE_KEY = 'hero-form-draft';

const loading = ref(false);
const saving = ref(false);
const error = ref<string | null>(null);

const form = ref<HeroInput>({
  nome: '',
  nomeHeroi: '',
  dataNascimento: '',
  altura: 0,
  peso: 0,
  superpoderIds: [],
});

const isEdit = computed(() => !!route.params.id);

const superpowers = ref<Superpower[]>([]);
const superpowersLoading = ref(false);
const superpowersError = ref<string | null>(null);

function goBack() {
  router.push({ name: 'HeroesList' });
}

function goToCreateSuperpower() {
  router.push({ name: 'SuperpowerCreate' });
}

async function loadSuperpowers() {
  superpowersLoading.value = true;
  superpowersError.value = null;

  try {
    superpowers.value = await getSuperpowers();
  } catch (err) {
    console.error(err);
    superpowersError.value = 'Erro ao carregar superpoderes.';
  } finally {
    superpowersLoading.value = false;
  }
}

async function loadHero() {
  if (!isEdit.value) return;

  loading.value = true;
  error.value = null;

  try {
    const id = Number(route.params.id);
    const hero = await getHero(id);

    form.value = {
      nome: hero.nome,
      nomeHeroi: hero.nomeHeroi,
      dataNascimento: hero.dataNascimento.substring(0, 10),
      altura: hero.altura,
      peso: hero.peso,
      superpoderIds: hero.superpoderes.map(sp => sp.id), 
    };
  } catch (err) {
    console.error(err);
    error.value = 'Erro ao carregar herói.';
  } finally {
    loading.value = false;
  }
}

function loadDraft() {
  if (isEdit.value) return; 

  const saved = localStorage.getItem(FORM_STORAGE_KEY);
  if (!saved) return;

  try {
    const parsed = JSON.parse(saved) as HeroInput;
    form.value = { ...form.value, ...parsed };
  } catch (e) {
    console.warn('Erro ao carregar rascunho do formulário', e);
  }
}

watch(
  form,
  newVal => {
    if (isEdit.value) return;
    localStorage.setItem(FORM_STORAGE_KEY, JSON.stringify(newVal));
  },
  { deep: true }
);

async function onSubmit() {
  saving.value = true;
  error.value = null;

  try {
    const payload = { ...form.value };

    if (isEdit.value) {
      const id = Number(route.params.id);
      await updateHero(id, payload);
    } else {
      await createHero(payload);
      localStorage.removeItem(FORM_STORAGE_KEY); 
    }

    router.push({ name: 'HeroesList' });
  } catch (err: any) {
    console.error("Erro ao salvar herói:", err);

    const apiMessage =
      err.response?.data?.message ||
      err.response?.data?.error ||
      err.response?.data?.title ||
      null;

    error.value = apiMessage || "Erro ao salvar herói.";
    alert(error.value);
  } finally {
    saving.value = false;
  }
}

onMounted(async () => {
  loadDraft();
  await Promise.all([loadSuperpowers(), loadHero()]);
});
</script>

<style scoped>
.page {
  min-height: 100vh;
  display: flex;
  justify-content: center;
  align-items: flex-start;
  padding: 48px 16px;
  background: #f3f4f6;
  font-family: system-ui, -apple-system, BlinkMacSystemFont, "Segoe UI", sans-serif;
}

.page__card {
  width: 100%;
  max-width: 720px;
  background: #ffffff;
  border-radius: 16px;
  padding: 32px 32px 24px;
  box-shadow:
    0 10px 30px rgba(15, 23, 42, 0.12),
    0 1px 2px rgba(15, 23, 42, 0.08);
}

.page__header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  gap: 16px;
  margin-bottom: 24px;
}

.page__title {
  margin: 0;
  font-size: 28px;
  font-weight: 600;
  color: #111827;
}

.page__subtitle {
  margin: 4px 0 0;
  font-size: 14px;
  color: #6b7280;
}

.page__status {
  font-size: 14px;
  color: #6b7280;
}

/* Form */

.form {
  display: flex;
  flex-direction: column;
  gap: 16px;
}

.form__row {
  display: flex;
  flex-wrap: wrap;
  gap: 16px;
}

.form__group {
  flex: 1;
  display: flex;
  flex-direction: column;
  gap: 6px;
}

.form__label {
  font-size: 14px;
  font-weight: 500;
  color: #374151;
}

.form__input {
  border-radius: 8px;
  border: 1px solid #d1d5db;
  padding: 8px 10px;
  font-size: 14px;
  transition: border-color 0.15s ease, box-shadow 0.15s ease;
}

.form__input:focus {
  outline: none;
  border-color: #2563eb;
  box-shadow: 0 0 0 1px rgba(37, 99, 235, 0.3);
}

.form__actions {
  margin-top: 8px;
}

.form__hint {
  font-size: 13px;
  color: #6b7280;
}

.form__hint--error {
  color: #b91c1c;
}

/* Superpoderes */

.superpowers-grid {
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
}

.superpower-pill {
  display: inline-flex;
  flex-direction: column;
  align-items: flex-start;
  gap: 2px;
  padding: 6px 10px;
  border-radius: 10px;
  border: 1px solid #e5e7eb;
  background: #f9fafb;
  cursor: pointer;
  font-size: 12px;
  min-width: 160px;
}

.superpower-pill input {
  margin-right: 6px;
}

.superpower-pill__name {
  font-weight: 600;
  color: #111827;
}

.superpower-pill__desc {
  color: #6b7280;
}

.empty-superpowers {
  display: flex;
  align-items: center;
  gap: 12px;
  margin-top: 4px;
}

/* Botões */

.btn {
  border-radius: 999px;
  padding: 8px 18px;
  font-size: 14px;
  font-weight: 500;
  cursor: pointer;
  border: none;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  gap: 6px;
  transition:
    background-color 0.15s ease,
    color 0.15s ease,
    box-shadow 0.15s ease,
    transform 0.05s ease;
}

.btn--primary {
  background: #2563eb;
  color: #ffffff;
  box-shadow: 0 8px 18px rgba(37, 99, 235, 0.35);
}

.btn--primary:hover {
  background: #1d4ed8;
  transform: translateY(-1px);
}

.btn--primary:disabled {
  opacity: 0.6;
  cursor: default;
  box-shadow: none;
  transform: none;
}

.btn--ghost {
  background: transparent;
  color: #4b5563;
  border: 1px solid #e5e7eb;
}

.btn--ghost:hover {
  background: #f9fafb;
}

.btn--sm {
  padding: 6px 14px;
  font-size: 13px;
}
</style>
