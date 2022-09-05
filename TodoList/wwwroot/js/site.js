$(() => {
    //var connection = new signalR.HubConnectionBuilder().withUrl("/signalServer").build();
    //connection.start();
    //connection.on("LoadProducts", function () {
    //    LoadData();
    //})
    LoadData();
    function LoadData() {
        var section = '';
        $.ajax({
            url: '/Home/GetAll',
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
                                    <button onclick="return Edit()" type="button" class="btn btn-sm btn-outline-success">click</button>
                                    <button type="button" class="btn btn-sm btn-outline-danger">Delete</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>`
                })
                $('#tasksection').html(section);
            },
            error: (error) => {
                console.log(error);
            }
        });

    };





});
function Active() {
    var name = $('#userName').val();
    var desc = $('#userDesc').val();

    var userData = new Object();
    userData.name = name;
    userData.description = desc;


    $.ajax({
        url: '/Home/Save',
        type: 'POST',
        data: userData,
        success(data) {
            if (data == 'pass') {
                $('#newModal').modal('hide');
                LoadData();
            } else {
                alert('error');
            }
        }
    });
};