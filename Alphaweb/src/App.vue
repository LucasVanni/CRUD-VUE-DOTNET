<script setup>
import Table from './components/Table.vue'
</script>

<template>
  <header>
    <h1>Welcome to my CRUD project to Consume .NET API</h1>
  </header>

  <main>
    <Table />
  </main>
</template>

<style scoped>
header {
  line-height: 1.5;
  padding: 10px; /* Reduzido para diminuir o tamanho */
  font-size: 16px; /* Ajuste conforme necessário */
}

@media (min-width: 1024px) {
  header {
    display: flex;
    align-items: center; /* Modificado de 'place-items' para 'align-items' */
    padding-right: calc(var(--section-gap) / 2);
    justify-content: flex-start; /* Mantém o conteúdo alinhado à esquerda */
  }

  main {
    width: 70%; /* Define a largura do container para 70% */
    margin: auto; /* Mantém o container centralizado */
    max-width: 1400px; /* Ajuste conforme necessário para controlar o tamanho máximo */
    padding: 20px;
    display: flex;
    flex-direction: column;
    flex: 1; /* Alterado de 2 para 1 para que flex-grow se comporte normalmente */
  }
}
</style>
