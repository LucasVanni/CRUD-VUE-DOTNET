<script setup>
import ConsumeAPI from './components/ConsumeAPI.vue'
</script>

<template>
  <header>
    <h1>Welcome to my CRUD project to Consume .NET API</h1>
  </header>

  <main>
    <ConsumeAPI />
  </main>
</template>

<style scoped>
header {
  line-height: 1.5;
}


@media (min-width: 1024px) {
  header {
    display: flex;
    place-items: center;
    padding-right: calc(var(--section-gap) / 2);
  }
}
</style>
