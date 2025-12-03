<template>
    <div class="page">
      <div class="page__card">
        <header class="page__header">
          <div>
            <h1 class="page__title">Novo Superpoder</h1>
            <p class="page__subtitle">
              Cadastre um superpoder para ser usado pelos heróis.
            </p>
          </div>
  
          <button type="button" class="btn btn--ghost" @click="goBack">
            Voltar
          </button>
        </header>
  
        <section v-if="saving" class="page__status">
          Salvando...
        </section>
  
        <section v-else>
          <form class="form" @submit.prevent="onSubmit">
            <div class="form__group">
              <label class="form__label" for="superpoder">Nome do Superpoder</label>
              <input
                id="superpoder"
                v-model="superpoder"
                type="text"
                class="form__input"
                required
                placeholder="Ex: Invisibilidade"
              />
            </div>
  
            <div class="form__group">
              <label class="form__label" for="descricao">Descrição</label>
              <textarea
                id="descricao"
                v-model="descricao"
                class="form__input form__textarea"
                rows="3"
                placeholder="Descreva brevemente o superpoder"
              ></textarea>
            </div>
  
            <div v-if="error" class="form__hint form__hint--error">
              {{ error }}
            </div>
  
            <div class="form__actions">
              <button type="submit" class="btn btn--primary">
                Cadastrar Superpoder
              </button>
            </div>
          </form>
        </section>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  import { ref } from 'vue';
  import { useRouter } from 'vue-router';
  import { createSuperpower } from '../api/superpowers';
  
  const router = useRouter();
  
  const superpoder = ref('');
  const descricao = ref('');
  const saving = ref(false);
  const error = ref<string | null>(null);
  
  function goBack() {
    router.back();
  }
  
  async function onSubmit() {
    saving.value = true;
    error.value = null;
  
    try {
      await createSuperpower({
        superpoder: superpoder.value,
        descricao: descricao.value,
      });
  
      router.push({ name: 'HeroCreate' });
    } catch (err: any) {
  console.error("Erro ao cadastrar superpoder.:", err);

  const apiMessage =
    err.response?.data?.message ||
    err.response?.data?.error ||
    err.response?.data?.title || // Para erros do .NET
    null;

  error.value = apiMessage || "Erro ao cadastrar superpoder.";
  alert(error.value);

} finally {
  saving.value = false;
}
  }
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
    max-width: 640px;
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
    font-size: 26px;
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
  
  .form__group {
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
  
  .form__textarea {
    resize: vertical;
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
  
  .btn--ghost {
    background: transparent;
    color: #4b5563;
    border: 1px solid #e5e7eb;
  }
  
  .btn--ghost:hover {
    background: #f9fafb;
  }
  </style>
  