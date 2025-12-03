<template>
  <div class="page">
    <div class="page__card">
      <header class="page__header">
        <div>
          <h1 class="page__title">Heróis cadastrados</h1>
          <p class="page__subtitle">
            Gerencie os heróis do sistema. Você pode criar, editar ou remover registros.
          </p>
        </div>

        <div class="header-actions">
          <button type="button" class="btn btn--primary" @click="goToCreate">
            + Novo Herói
          </button>

          <button type="button" class="btn btn--secondary" @click="goToCreateSuperpower">
            + Novo Superpoder
          </button>
        </div>
      </header>

      <section v-if="loading" class="page__status">
        Carregando heróis...
      </section>

      <section v-else-if="error" class="page__status page__status--error">
        {{ error }}
      </section>

      <section v-else>

        <div v-if="heroes.length">
          <!-- Filtro por ID -->
          <div class="heroes-filter">
            <label class="heroes-filter__label" for="searchId">Buscar por ID</label>
            <input
              id="searchId"
              v-model="searchId"
              type="text"
              class="heroes-filter__input"
              placeholder="Ex: 1 ou #1"
            />
          </div>

          <table class="heroes-table">
            <thead>
              <tr>
                <th>ID</th>
                <th>Nome</th>
                <th>Nome de Herói</th>
                <th>Altura</th>
                <th>Peso</th>
                <th>Data Nasc.</th>
                <th>Superpoderes</th>
                <th class="heroes-table__th-actions">Ações</th>
              </tr>
            </thead>

            <tbody>
              <tr v-for="hero in filteredHeroes" :key="hero.id">
                <td>#{{ hero.id }}</td>
                <td>{{ hero.nome }}</td>

                <td class="heroes-table__hero-name">
                  <span class="badge badge--hero">{{ hero.nomeHeroi }}</span>
                </td>

                <td>{{ hero.altura.toFixed(2) }} m</td>
                <td>{{ hero.peso.toFixed(1) }} kg</td>
                <td>{{ formatDate(hero.dataNascimento) }}</td>

                <td>
                  <span
                    v-for="sp in hero.superpoderes"
                    :key="sp.id"
                    style="margin-right: 6px;"
                  >
                    {{ sp.superpoder }}
                  </span>
                </td>

                <td class="heroes-table__actions">
                  <button type="button" class="btn btn--ghost" @click="editHero(hero.id)">
                    Editar
                  </button>
                  <button type="button" class="btn btn--danger" @click="removeHero(hero.id)">
                    Excluir
                  </button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>

        <div v-else class="empty-state">
          <h2 class="empty-state__title">Nenhum herói cadastrado ainda</h2>
          <p class="empty-state__subtitle">
            Comece cadastrando o primeiro herói para visualizar a lista aqui.
          </p>
          <button type="button" class="btn btn--primary" @click="goToCreate">
            Cadastrar primeiro herói
          </button>
        </div>
      </section>
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted, ref, computed } from 'vue';
import { useRouter } from 'vue-router';
import type { Hero } from '../types/hero';
import { getHeroes, deleteHero } from '../api/heroes';

const router = useRouter();

const heroes = ref<Hero[]>([]);
const loading = ref(false);
const error = ref<string | null>(null);


const searchId = ref('');


const filteredHeroes = computed(() => {
  const raw = searchId.value.trim();

  if (!raw) {
    return heroes.value;
  }


  const normalized = raw.replace('#', '');
  const id = Number(normalized);

  if (Number.isNaN(id)) {
    return heroes.value;
  }

  return heroes.value.filter((h) => h.id === id);
});

function formatDate(value: string): string {
  if (!value) return '';
  return new Date(value).toLocaleDateString('pt-BR');
}

async function loadHeroes() {
  loading.value = true;
  error.value = null;

  try {
    heroes.value = await getHeroes();
  } catch (err) {
    console.error(err);
    error.value = 'Erro ao carregar heróis. Verifique se a URL da API que você está executando localmente é a mesma configurada no arquivo http.ts. Caso o valor esteja diferente, atualize a constante API_BASE_URL para a URL correta que sua API está usando. Além disso, certifique-se de que o projeto HeroManager.Api está realmente em execução localmente e confirme se a URL/porta exibida no console corresponde ao endereço configurado no frontend.';
  } finally {
    loading.value = false;
  }
}

function goToCreate() {
  router.push({ name: 'HeroCreate' });
}

function goToCreateSuperpower() {
  router.push({ name: 'SuperpowerCreate' });
}

function editHero(id: number) {
  router.push({ name: 'HeroEdit', params: { id } });
}

async function removeHero(id: number) {
  if (!confirm('Tem certeza que deseja excluir este herói?')) {
    return;
  }

  try {
    await deleteHero(id);
    await loadHeroes();
  } catch (err) {
    console.error(err);
    alert('Erro ao excluir herói.');
  }
}

onMounted(loadHeroes);
</script>

<style scoped>
.page {
  min-height: 100vh;
  display: flex;
  justify-content: center;
  align-items: flex-start;
  padding: 48px 16px;
  background: #f3f4f6;
}

.page__card {
  width: 100%;
  max-width: 960px;
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

/* Botões lado a lado */
.header-actions {
  display: flex;
  gap: 12px;
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

.page__status--error {
  color: #b91c1c;
}


.heroes-filter {
  display: flex;
  justify-content: flex-end;
  align-items: center;
  gap: 8px;
  margin-bottom: 8px;
}

.heroes-filter__label {
  font-size: 13px;
  color: #6b7280;
}

.heroes-filter__input {
  padding: 6px 10px;
  border-radius: 999px;
  border: 1px solid #e5e7eb;
  font-size: 14px;
  outline: none;
}

.heroes-filter__input:focus {
  border-color: #2563eb;
  box-shadow: 0 0 0 1px rgba(37, 99, 235, 0.3);
}

/* Tabela */

.heroes-table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 8px;
  font-size: 14px;
}

.heroes-table th,
.heroes-table td {
  border-bottom: 1px solid #e5e7eb;
  padding: 10px 8px;
  text-align: left;
}

.heroes-table th {
  font-weight: 600;
  color: #4b5563;
  background: #f9fafb;
}

.heroes-table tbody tr:hover {
  background: #f3f4f6;
}

.heroes-table__th-actions {
  text-align: right;
}

.heroes-table__actions {
  text-align: right;
  white-space: nowrap;
}

.heroes-table__hero-name {
  font-weight: 500;
}

.badge {
  display: inline-flex;
  align-items: center;
  padding: 2px 8px;
  border-radius: 999px;
  font-size: 12px;
}

.badge--hero {
  background: rgba(37, 99, 235, 0.08);
  color: #1d4ed8;
}



.empty-state {
  padding: 32px 16px 16px;
  text-align: center;
}

.empty-state__title {
  margin: 0 0 8px;
  font-size: 20px;
  color: #111827;
}

.empty-state__subtitle {
  margin: 0 0 16px;
  font-size: 14px;
  color: #6b7280;
}



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

.btn--secondary {
  background: #10b981;
  color: #ffffff;
  box-shadow: 0 8px 18px rgba(16, 185, 129, 0.35);
}

.btn--secondary:hover {
  background: #059669;
  transform: translateY(-1px);
}

.btn--ghost {
  background: transparent;
  color: #4b5563;
  border: 1px solid #e5e7eb;
}

.btn--ghost:hover {
  background: #f9fafb;
}

.btn--danger {
  background: #ef4444;
  color: #ffffff;
}

.btn--danger:hover {
  background: #dc2626;
  transform: translateY(-1px);
}

.btn + .btn {
  margin-left: 8px;
}
</style>
