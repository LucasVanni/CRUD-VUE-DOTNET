
<template>
  <div class="modal-overlay" v-if="showModal">
    <div class="modal-content">
      <div class="modal-header">
        <h2>{{ editing ? 'Editando' : 'Cadastrando' }} um novo produto</h2>
        <button class="modal-close" @click="close">&times;</button>
      </div>
        <div class="form-group">
          <label for="nome">Nome:</label>
          <input type="text" id="nome" v-model="actualProduct.name" required>
        </div>
        <div class="form-group">
          <label for="preco">Preço (R$):</label>
          <input type="number" id="preco" v-model="actualProduct.price" required>
        </div>
        <div class="form-group">
          <label for="codigoDeBarras">Código de Barras:</label>
          <input type="text" id="codigoDeBarras" v-model="actualProduct.barCode" required>
        </div>
        <div class="form-group">
          <label for="imagem">Imagem:</label>
          <input  type="file" id="imagem" @change="loadingImage" accept="image/*">
        </div>
        <button @click="saveProduct">Salvar</button>
    </div>
  </div>
</template>


<script setup>
import { reactive, defineProps, defineEmits, watchEffect } from 'vue';

const props = defineProps({
  product: Object,
  showModal: Boolean,
  editing: Boolean
});

let actualProduct = reactive({
  name: '',
  price: '',
  barCode: '',
  image: ''
});


const emit = defineEmits(['close', 'save', 'edit']);

watchEffect(() => {
  if (props.editing && props.product) {
    actualProduct = reactive({
      name: props.product.name,
      price : props.product.price,
      barCode: props.product.barCode,
      image: props.product.imageBase64
    });
  }
});


const close = () => {
  emit('close');
};

const saveProduct = () => {
  if(!props.editing) {
    emit('save', {
      name: actualProduct.name,
      price: actualProduct.price,
      barCode: actualProduct.barCode,
      imageBase64: actualProduct.image
    });
  }
  else {
    emit('edit',{
      id: props.product.id,
      name: actualProduct.name,
      price: actualProduct.price,
      barCode: actualProduct.barCode,
      imageBase64: actualProduct.image
    });
  }
};

const loadingImage = (event) => {
  const file = event.target.files[0];
  if (file) {
    const reader = new FileReader();
    reader.onload = (e) => {
      actualProduct.image = e.target.result;
    };
    reader.readAsDataURL(file);
  }
};
</script>

<style scoped>
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}
.form-group {
  margin-bottom: 15px;
}

.modal-content {
  background-color: white;
  display: flex;
  flex-direction: column;
  padding: 50px;
  border-radius: 10px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  max-width: 500px;
  z-index: 1001;

  .modal-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 20px;
  }

  h2 {
    color: #4a4a4a;
    font-size: 24px;
    padding: 0 15px;
  }

  .modal-close {
    font-size: 20px;
    cursor: pointer;
  }
}




input[type="text"],
input[type="file"],
input[type="number"] {
  width: 100%;
  padding: 8px 12px;
  margin-top: 5px;
  box-sizing: border-box;
  border: 1px solid #ccc;
  border-radius: 4px;
}

input[type="text"]:focus,
input[type="file"]:focus {
  border-color: #90caf9;
  color: #90caf9;
  outline: none;
}

button {
  background-color: #007bff;
  color: white;
  padding: 10px 15px;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  transition: background-color 0.3s ease;
}

button:hover {
  background-color: #0056b3;
}

label {
  color: #4a4a4a;
  display: block;
  margin-top: 20px;
  font-weight: bold;
}


</style>