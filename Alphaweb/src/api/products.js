import api from "./index"

const getProducts = async (params) => {
    const response = await api.get("products", {params})
    if (response.status === 200) {
        return response.data
    } else {
        return {
            pagedProducts: [],
            totalProducts: 0
        }
    }
}

const deleteProduct = async (id) => {
    await api.delete(`products?id=${id}`)
}

const saveProduct = async (product) => {
    const response = await api.post("products", product)
    if (response.status === 200) {
        return response.data
    } else {
        return []
    }
}

const updateProduct = async (product) => {
    const response = await api.put("products", product)
    if (response.status === 200) {
        return response.data
    } else {
        return []
    }
}

export { getProducts, deleteProduct, saveProduct, updateProduct}