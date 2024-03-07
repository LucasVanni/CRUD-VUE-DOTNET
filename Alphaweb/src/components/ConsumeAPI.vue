

<template>
  <div class="container">
    <Modal :show-modal="showModal" :product="actualProduct" @close="showModal = false" @save="saveNewProduct" @edit="editProduct" />
    <div class="filters">
      <input v-model="nameFilter" type="text" placeholder="Nome do Produto" class="input" />
      <input v-model="barCodeFilter" type="text" placeholder="Código de Barras" class="input" />
      <button @click="() => showModal = true">Adicionar Produto</button>
    </div>

    <div class="filters">
      <div class="pagination-controls">
        <button @click="setPage(pagination.page - 1)" :disabled="pagination.page === 1">Anterior</button>
        <button @click="setPage(pagination.page + 1)" :disabled="pagination.page === Math.ceil(totalProducts / pagination.pageSize)">Próximo</button>
      </div>
      <div class="sort-controls">
        <button @click="sortByPrice">Ordenar por Preço</button>
        <button @click="() => fetchProducts(true)">Restaurar</button>
      </div>
    </div>

    <div>
      Mostrando itens {{ (pagination.page - 1) * pagination.pageSize + 1 }} a {{ Math.min(pagination.page * pagination.pageSize, totalProducts) }} de {{ totalProducts }}.
    </div>

    <div class="table-container">
        <table class="table">
          <thead>
          <tr>
            <th>Produto</th>
            <th>Preço</th>
            <th>Código de Barras</th>
            <th>Imagem</th>
            <th />
          </tr>
          </thead>
          <tbody>
          <tr v-for="product in filteredProducts" :key="product.barCode">
            <td>{{ product.name }}</td>
            <td>{{ product.price }}</td>
            <td>{{ product.barCode }}</td>
            <td><img :src="product.image" :alt="product.name" style="max-width: 100px;" class="product-image" /></td>
            <td>
              <button @click="deleteProductById(product.id)" class="delete-button">Excluir</button>
              <button @click="openModalEditProduct(product)">Editar</button>
            </td>
          </tr>
          </tbody>
        </table>
    </div>
  </div>
</template>


<script setup>
import { ref, computed, onMounted, reactive  } from 'vue';
import {deleteProduct, getProducts, saveProduct, updateProduct} from "@/api/products.js";
import Modal from "@/components/Modal.vue";


const products = ref([]);
const totalProducts = ref(0);
const nameFilter = ref('');
const barCodeFilter = ref('');
const showModal = ref(false);
const actualProduct = ref({});

const filteredProducts = computed(() =>
    products.value.filter((product) => (nameFilter.value ? product.name.toLowerCase().includes(nameFilter.value.toLowerCase()) : true) &&
        (barCodeFilter.value ? product.barCode.includes(barCodeFilter.value) : true)
    ));

const pagination = reactive({
  page: 1,
  pageSize: 10,
  sortBy: ''
});

async function fetchProducts(clearAllPagination = false) {
  try {
    const response = await getProducts( clearAllPagination ? pagination :{
      page: pagination.page,
      pageSize: pagination.pageSize,
      sortBy: ''
    });
    products.value = response.pagedProducts;
    totalProducts.value = response.totalProducts;
  } catch (error) {
    console.error('Erro ao carregar os produtos:', error);
  }
}


const setPage = async (newPage)  => {
  pagination.page = newPage;
  await fetchProducts();
}

const sortByPrice = async () => {
  pagination.sortBy = pagination.sortBy === 'price' ? 'price_desc' : 'price';
  await fetchProducts();
}


onMounted(fetchProducts);


const deleteProductById = async (id) => {
  try {
    await deleteProduct(id);
    await fetchProducts();
  } catch (error) {
    console.error('Erro ao excluir o produto:', error);
  }
}

const saveNewProduct = async (data) => {
  try {
    showModal.value = false;
    await saveProduct(data);
    await fetchProducts();
  } catch (error) {
    console.error('Erro ao salvar o produto:', error);
  }
}

const openModalEditProduct = (data) => {
  try {
    showModal.value = true;
    actualProduct.value = data;
  } catch (error) {
    console.error('Erro ao editar o produto:', error);
  }
}

const editProduct = async (data) => {
  try {
    showModal.value = false;
    await updateProduct(data);
    await fetchProducts();
  } catch (error) {
    console.error('Erro ao editar o produto:', error);
  }
}


</script>

<style scoped>
.container {
  max-width: 1200px;
  margin: auto;
  padding: 20px;
}

.filters {
  margin-bottom: 20px;
}

.input {
  padding: 10px;
  margin-right: 10px;
  border: 1px solid #ccc;
  border-radius: 5px;
}

.table-container {
  max-height: 700px;
  overflow-y: auto;
  border: 1px solid #ccc;
}

.table {
  width: 100%;
  border-collapse: collapse;
}

.table th, .table td {
  text-align: left;
  padding: 8px;
  border-bottom: 1px solid #ddd;
}

.table th {
  background-color: #4a4a4a; /* Cor de fundo escura */
  color: white; /* Cor do texto claro */
  text-align: left;
  padding: 8px;
  border-bottom: 1px solid #ddd;
}

.product-image {
  width: 100px;
  height: auto;
}
</style>
