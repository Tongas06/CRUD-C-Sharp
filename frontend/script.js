document.addEventListener("DOMContentLoaded", search);
const URL_API = 'https://localhost:7246/api/'

let customers = []

function init() {
    search()
}

function agregar() {
    clean()
    abrirFormulario()
}

function abrirFormulario() {
    htmlModal = document.getElementById("modal")
    htmlModal.setAttribute("class", "modale opened")
}

function cerrarModal() {
    htmlModal = document.getElementById("modal");
    htmlModal.setAttribute("class", "modale");
}

async function search() {
    var url = URL_API + 'Customer'
    var response = await fetch(url, {
      "method": 'GET',
      "headers": {
        "Content-Type": 'application/json'
      }
    })
    customers = await response.json();
  
    var html = ''

    for (customer of customers) {
        let row = `<tr>
        <td>${customer.firstName}</td>
        <td>${customer.lastName}</td>
        <td>${customer.email}</td>
        <td>${customer.phone}</td>
        <td>
            <a href="#" onclick="edit(${customer.id})" class="myButton">Editar</a>
            <a href="#" onclick="remove(${customer.id})" class="btnDelete">Eliminar</a>
        </td>
    </tr>`

        html = html + row

    }

    document.querySelector('#customers > tbody').outerHTML = html
}

async function remove(id) {
    respuesta = confirm('¿Está seguro de eliminarlo?')
    if (respuesta) {
        var url = URL_API + 'Customer/' + id
        await fetch(url, {
            "method": 'DELETE',
            "headers": {
                "Content-Type": 'application/json'
            }
        })
        window.location.reload();
    }
}


function clean() {
    document.getElementById('txtId').value = ''
    document.getElementById('txtFirstname').value = ''
    document.getElementById('txtLastname').value = ''
    document.getElementById('txtPhone').value = ''
    document.getElementById('txtAddress').value = ''
    document.getElementById('txtEmail').value = ''
}

async function save() {
    var data = {
        "address": document.getElementById('txtAddress').value,
        "email": document.getElementById('txtEmail').value,
        "firstname": document.getElementById('txtFirstname').value,
        "lastname": document.getElementById('txtLastname').value,
        "phone": document.getElementById('txtPhone').value
    }

    var id = document.getElementById('txtId').value
    if (id != '') {
        data.id = id
    }

    var url = URL_API + 'Customer'
    await fetch(url, {
        "method": id != '' ? 'PUT' : 'POST',
        "body": JSON.stringify(data),
        "headers": {
            "Content-Type": 'application/json'
        }
    })
    window.location.reload();
}

function edit(id) {
    let customer = customers.find(c => c.id === id);
  
    if (customer) {
      document.getElementById('txtId').value = customer.id;
      document.getElementById('txtFirstname').value = customer.firstName;
      document.getElementById('txtLastname').value = customer.lastName;
      document.getElementById('txtPhone').value = customer.phone;
      document.getElementById('txtAddress').value = customer.address;
      document.getElementById('txtEmail').value = customer.email;
  
      abrirFormulario();
    }
  }
  
  async function remove(id) {
    const respuesta = confirm('¿Está seguro de eliminarlo?');
    if (respuesta) {
      const url = URL_API + 'Customer/' + id;
      await fetch(url, {
        method: 'DELETE',
        headers: {
          'Content-Type': 'application/json'
        }
      });
      window.location.reload();
    }
  }







