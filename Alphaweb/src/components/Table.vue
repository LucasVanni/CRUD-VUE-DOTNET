

<template>

  <div class="container">
    <Modal :show-modal="showModal" :editing="editing" :product="actualProduct" @close="showModal = false" @save="saveNewProduct" @edit="editProduct" />
    <div class="filters">
      <input v-model="nameFilter" type="text" placeholder="Nome do Produto" class="input" />
      <input v-model="barCodeFilter" type="text" placeholder="Código de Barras" class="input" />
      <button @click="showAddProductModal">Adicionar Produto</button>
    </div>

    <div class="filters-pagination">
      <div class="pagination-controls">
        <button @click="setPage(pagination.page - 1)" :disabled="pagination.page === 1">Anterior</button>
        <button @click="setPage(pagination.page + 1)" :disabled="pagination.page === Math.ceil(totalProducts / pagination.pageSize)">Próximo</button>
      </div>
      <div class="sort-controls">
        {{pagination.sortBy}}
        <button @click="restoreOrSortInfos">Ordenar por Preço {{ pagination.sortBy === '' ? '' :  pagination.sortBy === 'price' ? 'Crescente' : 'Decrescente' }}</button>
        <button @click="restoreOrSortInfos({restore: true})">Restaurar</button>
      </div>
    </div>

    <div>
      Mostrando itens
      {{ Math.min((pagination.page - 1) * pagination.pageSize + 1, totalProducts) }}
      a
      {{ Math.min(pagination.page * pagination.pageSize, totalProducts) }}
      de
      {{ totalProducts }}.
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
          <tr v-if="loading" >
            <p>Carregando...</p>
         </tr>
          <tr v-else v-for="product in filteredProducts" :key="product.barCode">
            <td>{{ product.name }}</td>
            <td>{{ product.price }}</td>
            <td>{{  product.barCode ? product.barCode : 'Não informado' }}</td>
            <td><img :src="product.imageBase64" :alt="product.name" style="max-width: 100px;" class="product-image" /></td>
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
const editing = ref(false);
const loading = ref(false);

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
    loading.value = true;

    const response = await getProducts({
      page: pagination.page,
      pageSize: pagination.pageSize,
      sortBy: clearAllPagination ? '' : pagination.sortBy,
    });
    products.value = response.pagedProducts;
    totalProducts.value = response.totalProducts;
    loading.value = false;
  } catch (error) {
    console.error('Erro ao carregar os produtos:', error);
  }
}


const setPage = async (newPage)  => {
  pagination.page = newPage;
  await fetchProducts();
}

const restoreOrSortInfos = async ({restore = false}) => {
  if (restore) {
    pagination.sortBy = '';
    pagination.page = 1;
    await fetchProducts();
    return;
  }

  pagination.sortBy = pagination.sortBy === 'price' ? 'price_desc' : 'price';
  pagination.page = 1;
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
    actualProduct.value = {};
    await saveProduct(data);
    await fetchProducts();
  } catch (error) {
    console.error('Erro ao salvar o produto:', error);
  }
}

const showAddProductModal = () => {
  editing.value = false;
  actualProduct.value = {};
  showModal.value = true;
}

const openModalEditProduct = (data) => {
  try {
    editing.value = true;
    actualProduct.value = data;
    showModal.value = true;
  } catch (error) {
    console.error('Erro ao editar o produto:', error);
  }
}

const editProduct = async (data) => {
  try {
    showModal.value = false;
    await updateProduct(data);
    await fetchProducts();
    actualProduct.value = {};
  } catch (error) {
    console.error('Erro ao editar o produto:', error);
  }
}
</script>

<style scoped>
.container {
  margin: auto;
  padding: 20px;
  display: flex;
  flex-direction: column;
  flex: 2;
}

button {
  background-color: #007bff;
  color: white;
  padding: 10px 15px;
  margin: 2px 5px;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  transition: background-color 0.3s ease;
}

button:hover {
  background-color: #0056b3;
}

.filters {
  margin-bottom: 20px;

  button {
    background-color: #28a745;
    color: white;
    padding: 10px 15px;
    margin: 2px 5px;
    border: none;
    border-radius: 5px;
    cursor: pointer;
    transition: background-color 0.3s ease;
  }

  button:hover {
    background-color: #218838;
  }
}

.filters-pagination {
  display: flex;
  justify-content: space-between;
  margin-bottom: 20px;

  Button {
    margin-right: 10px;
  }
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
  width: 250px;
}

.table th {
  background-color: #4a4a4a;
  color: white;
  text-align: left;
  padding: 8px;
  border-bottom: 1px solid #ddd;
}

.product-image {
  width: 100px;
  height: auto;
}
</style>