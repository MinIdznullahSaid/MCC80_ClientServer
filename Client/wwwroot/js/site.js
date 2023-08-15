// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

/*let b1 = document.querySelector("#b1");
let b2 = document.querySelector("#b2");
let b3 = document.querySelector("#b3");
let b4 = document.querySelector("#b4");

b1.addEventListener('click', () => {
    let a = document.getElementById("what");
    a.style.backgroundColor = "blue";
})

b2.addEventListener('click', () => {
    let a = document.getElementById("where");
    a.style.backgroundColor = "red";
})

b3.addEventListener('click', () => {
    let a = document.getElementById("why");
    a.style.backgroundColor = "yellow";
})

b4.addEventListener('click', () => {
    let a = document.getElementById("what");
    a.style.backgroundColor = "";
    let b = document.getElementById("where");
    b.style.backgroundColor = "";
    let c = document.getElementById("why");
    c.style.backgroundColor = "";
})*/

//asynchronous javascript
$(document).ready(function () {
    $('#employeeTable').DataTable({
        dom: 'Bfrtip',
        buttons: [
            'colvis', 'copy', 'csv', 'excel', 'pdf', 'print'
        ]    
    });
});

$(document).ready(function () {
    let table = new DataTable('#myTable', {
        ajax: {
            url: "https://localhost:7149/api/employees",
            dataSrc: "data",
            dataType: "JSON"
        },
        dom: 'Bfrtip',
        buttons: [
            'colvis','copy', 'csv', 'excel', 'pdf', 'print'
        ],
        columns: [
            {
                data: "",
                render: function (data, type, row, meta) {
                    return meta.row+1;
                }
            },
            {
                data: "",
                render: function (data, type, row) {
                    return `${row.firstName} ${row.lastName}`;
                }
            },
            {
                data: 'nik'
            },
            {
                data: 'gender',
                render: function (data) {
                    return data === 0 ? 'Male' : 'Female';
                }
            },
            {
                data: 'birthDate',
                render: function (data) {
                    const date = new Date(data);
                    const formattedDate = date.toISOString().slice(0, 10);
                    return formattedDate;
                }
            },
            {
                data: 'email'
            },
            {
                data: 'phoneNumber'
            },
            {
                data: 'hiringDate',
                render: function (data) {
                    const date = new Date(data);
                    const formattedDate = date.toISOString().slice(0, 10);
                    return formattedDate;
                }
            },
            {
                data: '',
                render: function (data, type, row) {
                    return `
                    <button onclick="Delete('${row.guid}')" class="btn btn-danger">Delete</button>`
                }
            }
        ]
    });
});

function Delete(guid) {
    $.ajax({
        url: "https://localhost:7149/api/employees?guid=" + guid,
        type: "DELETE",
        success: function (result) {
            alert("Delete successfully");
        }
    })
}

$(document).ready(function () {
    $.ajax({
        url: "https://localhost:7149/api/employees"
    }).done((result) => {
        let maleCount = 0;
        let femaleCount = 0;

        result.data.forEach(function (dataDetail) {
            if (dataDetail.gender === 1) {
                maleCount++;
            } else {
                femaleCount++;
            }
        });

        const xValues = ["Male", "Female"];
        const yValues = [maleCount, femaleCount];
        const barColors = [
            "#1982c3",
            "#ff4e6a"
        ];

        new Chart("genderChart", {
            type: "pie",
            data: {
                labels: xValues,
                datasets: [{
                    backgroundColor: barColors,
                    data: yValues
                }]
            },
            options: {
                title: {
                    display: true,
                    text: "Employees Gender Pescentage"
                }
            }
        });
    });  
});

$(document).ready(function () {
    $.ajax({
        url: "https://localhost:7149/api/employees"
    }).done((result) => {
        let age18To30Count = 0;
        let above30Count = 0;

        const currentDate = new Date();
        result.data.forEach(function (dataDetail) {
            const birthDate = new Date(dataDetail.birthDate);
            const age = currentDate.getFullYear() - birthDate.getFullYear();

            if (age >= 18 && age <= 30) {
                age18To30Count++;
            } else {
                above30Count++;
            }
        });

        const xValues = ["18-30", "Above 30", ""];
        const yValues = [age18To30Count, above30Count, 0];
        const barColors = [
            "#ebe2cf",
            "#aaaaaa",
            "#eeeeee"
        ];

        new Chart("ageChart", {
            type: "bar",
            data: {
                labels: xValues,
                datasets: [{
                    backgroundColor: barColors,
                    data: yValues
                }]
            },
            options: {
                scales: {
                    y: {
                        beginAtZero: true
                    }
                },
                legend: { display: false },
                title: {
                    display: true,
                    text: "Age Percentage"
                }
            }
        });
    });
});

function Insert() {
    var obj = new Object(); //sesuaikan sendiri nama objectnya dan beserta isinya
    //ini ngambil value dari tiap inputan di form nya
    obj.firstName = $("#firstName").val();
    obj.lastName = $("#lastName").val();
    obj.gender = parseInt($("#gender").val());
    obj.birthDate = $("#birthDate").val();
    obj.hiringDate = $("#hiringDate").val();
    obj.email = $("#email").val();
    obj.phoneNumber = $("#phoneNumber").val();
    //isi dari object kalian buat sesuai dengan bentuk object yang akan di post
    $.ajax({
        url: "https://localhost:7149/api/employees",
        type: "POST",
        data: JSON.stringify(obj), //jika terkena 415 unsupported media type (tambahkan headertype Json & JSON.Stringify();)
        contentType: "application/json",
}).done((result) => {
    alert("Register successfully"); //alert pemberitahuan jika success
}).fail((error) => {
    alert("Register failed"); //alert pemberitahuan jika gagal
})
}



/*$.ajax({
    url: "https://pokeapi.co/api/v2/pokemon?limit=100&offset=0"
}).done((result) => {
    let temp = "";
    $.each(result.results, (key, value) => {
        temp += `
                <tr>
                    <td bgcolor="white">${key + 1}</td>
                    <td bgcolor="white" align="center" width="60%">${capitalizeFirstLetter(value.name)}</td>
                    <td bgcolor="white" ><button onclick="detailPM('${value.url}')" data-bs-toggle="modal" data-bs-target="#modalPM" class="btn btn-primary">Detail</button></td>
                </tr>
        `;
    });
    $("#tbodyPM").html(temp);
});
function capitalizeFirstLetter(str) {
    return str.charAt(0).toUpperCase() + str.slice(1);
}

const colorBadge= {
    grass: '#4BE632',
    poison: '#63169F',
    fire: '#E56D00',
    flying: '#F3D3A1',
    water: '#45E9C1',
    bug: '#C68E3F',
    normal: '#BDCCC1',
    electric: '#FCE303',
    ground: '#945C03',
    fairy: '#F77EE7',
    fighting: '#EDE6A6',
    psychic: '#9C3FA1',
    rock: '#575757'
}
    
function badgeType(badge) {
    for (let type in colorBadge) {
        if (type === badge) {
            return colorBadge[type];
        }
    }
}

function detailPM(stringURL) {
    $.ajax({
        url: stringURL,
        success: (result) => {
            $('.modal-title').html("Pokemon Detail");
            $('.modal-body').html(`
            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-4 my-auto">
                        <img src="` + result.sprites.other['official-artwork'].front_default + `" class="img-fluid my-auto" width="200" height="200"/>
                    </div>

                    <div class="col-md-8">
                        <ul class="list-group list-group-horizontal">
                            <li class="list-group-item border-0"><h3>` + capitalizeFirstLetter(result.name) + `</h3></li>
                        </ul>
                        <ul class="list-group list-group-horizontal mb-1">
                            <li class="list-group-item"> <b>Type</b>:<br></br>${result.types.map(type => `
                                <span class="badge" style="color: white; background-color: ${badgeType(type.type.name)}">${type.type.name}</span>
                            `).join('')}</li>                           
                            <li class="list-group-item"> <b>Ability</b>:<br></br>${result.abilities.map(ability => `
                                <span class="badge text-bg-light" style="color: black; background-color: #def1f9">${ability.ability.name}</span>
                            `).join('')}</li>
                        </ul>
                        <ul class="list-group list-group-horizontal mb-1">
                            <li class="list-group-item"> <b>Height</b>: `+ (result.height)/10 + ` m</li>
                            <li class="list-group-item"> <b>Weight</b>: `+ result.weight + ` lbs</li>
                        </ul>
                    </div>

                    <div class="healthPoint" style="font-size: 12px">Health Point
                        <div class="progress" role="progressbar" aria-valuenow="`+ result.stats[0].base_stat +`" aria-valuemin="0" aria-valuemax="100">
                            <div class="progress-bar" style="background-color: #7CFC00; width: `+ result.stats[0].base_stat + `%">` + result.stats[0].base_stat +`</div>
                        </div>
                    </div>
                    <div class="attack" style="font-size: 12px">Attack
                        <div class="progress" role="progressbar" aria-valuenow="`+ result.stats[1].base_stat + `" aria-valuemin="0" aria-valuemax="100">
                            <div class="progress-bar" style="background-color: #FF0000; width: `+ result.stats[1].base_stat + `%">` + result.stats[1].base_stat +`</div>
                        </div>
                    </div>
                    <div class="defense" style="font-size: 12px">Defense
                        <div class="progress" role="progressbar" aria-valuenow="`+ result.stats[2].base_stat + `" aria-valuemin="0" aria-valuemax="100">
                            <div class="progress-bar" style="background-color: #00FFFF; width: `+ result.stats[2].base_stat + `%">` + result.stats[2].base_stat +`</div>
                        </div>
                    </div>
                     <div class="specialAttack" style="font-size: 12px">Special Attack
                        <div class="progress" role="progressbar" aria-valuenow="`+ result.stats[3].base_stat + `" aria-valuemin="0" aria-valuemax="100">
                            <div class="progress-bar" style="background-color: #B22222; width: `+ result.stats[3].base_stat + `%">` + result.stats[3].base_stat +`</div>
                        </div>
                    </div>
                    <div class="specialDefense" style="font-size: 12px">Special Defense
                        <div class="progress" role="progressbar" aria-valuenow="`+ result.stats[4].base_stat + `" aria-valuemin="0" aria-valuemax="100">
                            <div class="progress-bar" style="background-color: #1E90FF; width: `+ result.stats[4].base_stat + `%">` + result.stats[4].base_stat +`</div>
                        </div>
                    </div>
                    <div class="speed" style="font-size: 12px">Speed
                        <div class="progress" role="progressbar" aria-valuenow="`+ result.stats[5].base_stat + `" aria-valuemin="0" aria-valuemax="100">
                            <div class="progress-bar" style="background-color: #FFFF00; width: `+ result.stats[5].base_stat + `%">` + result.stats[5].base_stat +`</div>
                        </div>
                    </div>
                </div>
            </div>
            `);
        }
    });
}*/

