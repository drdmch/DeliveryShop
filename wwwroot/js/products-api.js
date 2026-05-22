const uri = 'api/Products';
let products = [];

function getProducts() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayProducts(data))
        .catch(error => console.error('Unable to get products.', error));
}

function deleteProduct(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE'
    })
    .then(() => getProducts())
    .catch(error => console.error('Unable to delete product.', error));
}

function displayEditForm(id) {
    const product = products.find(p => p.id === id);
    document.getElementById('edit-id').value = product.id;
    document.getElementById('edit-title').innerText = product.name;
    document.getElementById('edit-price').value = product.price;
    document.getElementById('edit-stock').value = product.stockQuantity;
    document.getElementById('editProductCard').style.display = 'block';
}

function updateProduct() {
    const productId = document.getElementById('edit-id').value;
    const product = {
        id: parseInt(productId, 10),
        price: parseFloat(document.getElementById('edit-price').value),
        stockQuantity: parseInt(document.getElementById('edit-stock').value, 10)
    };

    fetch(`${uri}/${productId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(product)
    })
    .then(() => getProducts())
    .catch(error => console.error('Unable to update product.', error));

    closeInput();
    return false;
}

function closeInput() {
    document.getElementById('editProductCard').style.display = 'none';
}

function _displayProducts(data) {
    const tBody = document.getElementById('productsTable');
    tBody.innerHTML = '';

    const counterParagraph = document.getElementById('counter');
    counterParagraph.innerText = `Всього товарів у каталозі API: ${data.length}`;

    data.forEach(product => {
        let editButton = document.createElement('button');
        editButton.className = 'btn btn-sm btn-outline-primary me-2';
        editButton.innerHTML = '<i class="bi bi-pencil"></i>';
        editButton.setAttribute('onclick', `displayEditForm(${product.id})`);

        let deleteButton = document.createElement('button');
        deleteButton.className = 'btn btn-sm btn-outline-danger';
        deleteButton.innerHTML = '<i class="bi bi-trash"></i>';
        deleteButton.setAttribute('onclick', `deleteProduct(${product.id})`);

        let tr = tBody.insertRow();
        
        let td1 = tr.insertCell(0);
        td1.className = 'fw-bold text-dark';
        td1.appendChild(document.createTextNode(product.name));

        let td2 = tr.insertCell(1);
        td2.appendChild(document.createTextNode(`${product.price} грн`));

        let td3 = tr.insertCell(2);
        td3.appendChild(document.createTextNode(product.stockQuantity));

        let td4 = tr.insertCell(3);
        td4.className = 'text-center';
        td4.appendChild(editButton);
        td4.appendChild(deleteButton);
    });

    products = data;
}