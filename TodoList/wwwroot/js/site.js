$(() => {
    var connection = new signalR.HubConnectionBuilder().withUrl("/taskServer").build();
    connection.start();
    connection.on("LoadData", function () {
        LoadPending();
    });
    LoadPending();
})


function Clear() {
    $('#Title').val('');
    $('#Task').val('');
    $('#Date').val('');
    $("#canvaClose").trigger("click");

}

function LoadPending() {
    var section = '';
    $.ajax({
        url: '/Home/GetPending',
        method: 'GET',
        success: (result) => {
            $.each(result, (k, v) => {
                var date = new Date(v.date);
                var formatdate = ((date.getMonth() > 8) ? (date.getMonth() + 1) : ('0' + (date.getMonth() + 1))) + '/' + ((date.getDate() > 9) ? date.getDate() : ('0' + date.getDate())) + '/' + date.getFullYear();
                section += `
                <div class="mb-2 d-flex card border border-dark border-5 border-top-0 border-end-0 border-bottom-0">
                    <div class="card-body">
                        <h4 class="card-title">${v.title}</h4>
                        <p class="card-text">${v.task}</p>
                    </div>
                    <div class="card-footer">
                        <div class="row">
                            <div class="col-6 border-end">
                                <i class="bi bi-calendar-week"></i> ${formatdate}
                            </div>
                            <div class="col-6">
                                <div class="float-end">
                                    <button id="toActive" value="${v.id}" type="button" class="btn btn-sm btn-outline-success">Active</button>
                                    <button id="toRemove" value="${v.id}" type="button" class="btn btn-sm btn-outline-danger">Delete</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>`
            })
            $('#pendingsection').html(section);
        },
        error: (error) => {
            console.log(error);
        }
    });
}
function LoadActive() {
    var section = '';
    $.ajax({
        url: '/Home/GetActive',
        method: 'GET',
        success: (result) => {
            $.each(result, (k, v) => {
                var date = new Date(v.date);
                var formatdate = ((date.getMonth() > 8) ? (date.getMonth() + 1) : ('0' + (date.getMonth() + 1))) + '/' + ((date.getDate() > 9) ? date.getDate() : ('0' + date.getDate())) + '/' + date.getFullYear();
                section += `
                <div class="mb-2 d-flex card border border-warning border-5 border-top-0 border-end-0 border-bottom-0">
                    <div class="card-body">
                        <h4 class="card-title">${v.title}</h4>
                        <p class="card-text">${v.task}</p>
                    </div>
                    <div class="card-footer">
                        <div class="row">
                            <div class="col-6 border-end">
                                <i class="bi bi-calendar-week"></i> ${formatdate}
                            </div>
                            <div class="col-6">
                                <div class="float-end">
                                    <button id="toPending" value="${v.id}" type="button" class="btn btn-sm btn-outline-dark">Back to list</button>
                                    <button id="toDone" value="${v.id}" type="button" class="btn btn-sm btn-outline-success">Done</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>`
            })
            $("#activesection").html(section);
        },
        error: (error) => {
            console.log(error);
        }
    });
};
function LoadDone() {
    var section = '';
    $.ajax({
        url: '/Home/GetDone',
        method: 'GET',
        success: (result) => {
            $.each(result, (k, v) => {
                var date = new Date(v.date);
                var formatdate = ((date.getMonth() > 8) ? (date.getMonth() + 1) : ('0' + (date.getMonth() + 1))) + '/' + ((date.getDate() > 9) ? date.getDate() : ('0' + date.getDate())) + '/' + date.getFullYear();
                section += `
                <div class="mb-2 d-flex card border border-success border-5 border-top-0 border-end-0 border-bottom-0">
                    <div class="card-body">
                        <h4 class="card-title">${v.title}</h4>
                        <p class="card-text">${v.task}</p>
                    </div>
                    <div class="card-footer">
                        <div class="row">
                            <div class="col-6 border-end">
                                <i class="bi bi-calendar-week"></i> ${formatdate}
                            </div>
                            <div class="col-6">
                                <div class="float-end">
                                    <button id="toPending" value="${v.id}" type="button" class="btn btn-sm btn-outline-dark">Back to list</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>`
            })
            $("#donesection").html(section);
        },
        error: (error) => {
            console.log(error);
        }
    });
};

function Add() {
    var title = $('#Title').val();
    var task = $('#Task').val();
    var date = $('#Date').val();


    var objData = new Object();
    objData.title = title;
    objData.task = task;
    objData.date = date;
    objData.status = "pending";

    $.ajax({
        url: '/Home/Save',
        type: 'POST',
        data: objData,
        success(data) {
            if (data == "pass") {
                LoadPending();

                Clear();
            }

        },
        error(error) {
            console.log(error);
        }
    });
};

//convert to pending
$(document).on('click', '#toPending', function () {
    var objData = new Object();
    objData.id = this.value;
    objData.status = "pending";

    $.ajax({
        url: '/Home/Save',
        type: 'POST',
        data: objData,
        success(data) {
            LoadActive();
            LoadPending();
            LoadDone();
            console.log(data);
        },
        error(error) {
            console.log(error);
        }
    });

});

//convert to active
$(document).on('click', '#toActive', function ()
{
    var objData = new Object();
    objData.id = this.value;
    objData.status = "active";

    $.ajax({
        url: '/Home/Save',
        type: 'POST',
        data: objData,
        success(data) {
            LoadActive();
            LoadPending();
            LoadDone();
            console.log(data);
        },
        error(error) {
            console.log(error);
        }
    });
   
});

// convert to done
$(document).on('click', '#toDone', function () {
    var objData = new Object();
    objData.id = this.value;
    objData.status = "done";

    $.ajax({
        url: '/Home/Save',
        type: 'POST',
        data: objData,
        success(data) {
            LoadActive();
            LoadPending();
            LoadDone();
            console.log(data);
        },
        error(error) {
            console.log(error);
        }
    });

});
$(document).on('click', '#toRemove', function () {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Home/Remove/' + this.value,
                type: 'POST',
                data: { id: this.value },
                success: function (data) {
                    LoadPending();
                },
                error: function (err) {
                    alert(err.responseText);
                }
            });
            return false;
      
        }
    })

});

