
    jQuery(document).ready(function ($) {
        $(function () {
            $("#id").val(0);

            $('#example').DataTable({});

            $('#employee-form').on('submit', function (e) {
                e.preventDefault();
                var id = $("#id").val();
                if (id == 0) {
                    var postData = {
                        "name": $("#name").val(),
                        "email": $("#email").val(),
                        "age": $("#age").val(),
                        "designation": $("#designation").val()
                    }
                    swal({
                        title: 'Do you want to Save ?',
                        showCancelButton: true,
                        showConfirmButton: true,
                        confirmButtonText: 'Yes',
                        cancelButtonText: 'No',
                        confirmButtonClass: 'btn btn-primary',
                        cancelButtonClass: 'btn btn-danger',
                        icon: 'warning',
                        buttonsStyling: true,
                        buttons: true,
                        buttons: ["No", "Yes"]
                    }).then(function (yes) {
                        if (yes) {
                            $.ajax({
                                type: "POST",
                                url: '/api/test/employeeInsert',
                                contentType: 'application/json',
                                data: JSON.stringify(postData),
                                beforeSend: function () {
                                    Show();
                                },
                                success: function (data) {
                                    console.log(data);
                                    Hide();
                                    $.ajax({
                                        type: 'GET',
                                        url: '/api/test',
                                        dataType: 'JSON',
                                        success: function (data) {
                                            $('#example').dataTable().fnClearTable();
                                            $('#example').dataTable().fnDestroy();

                                            $.each(data, function (index) {
                                                $('#example tbody').append('<tr><td>' + data[index]["name"] + '</td><td>' + data[index]["email"] + '</td><td>' + data[index]["age"] + '</td><td>' + data[index]["designation"] + '</td><td> <a href="" class="btn btn-primary btn-sm" type="button" onclick="editEmployee(' + data[index]["id"] + ')">Edit</a>  <button type="button" onclick="deleteEmployee(' + data[index]["id"] + ')" class="btn btn-danger btn-sm">Delete</button></td></tr>');
                                            });
                                            $('#example').DataTable({
                                                "destroy": true,
                                            });
                                            // remove previously loaded options

                                        }
                                    });
                                    swal("Message", "Data Saved Successfully!", "success");
                                },
                                error: function () {
                                    swal("Message", "Something went wrong!", "error");
                                }
                            });
                        }
                    });
                }
                else {
                    update();
                }
            });

            $.ajax({
                type: 'GET',
                url: '/api/test/Designation',
                dataType: 'JSON',
                success: function (data) {
                    console.log(data);
                    // remove previously loaded options
                    for (i in data) {
                        $('#designation').append('<option value = ' + data[i].id + '>' + data[i].name + '</option>');
                    }
                }
            });

            $.ajax({
                type: 'GET',
                url: '/api/test',
                dataType: 'JSON',
                success: function (data) {
                    $('#example').dataTable().fnClearTable();
                    $('#example').dataTable().fnDestroy();
                    $.each(data, function (index) {
                        $('#example tbody').append('<tr><td>' + data[index]["name"] + '</td><td>' + data[index]["email"] + '</td><td>' + data[index]["age"] + '</td><td>' + data[index]["designation"] + '</td><td> <button type="button" onclick="editEmployee(' + data[index]["id"] +')" class="btn btn-primary btn-sm">Edit</button> <button type="button" onclick="deleteEmployee(' + data[index]["id"] +')" class="btn btn-danger btn-sm">Delete</button></td></tr>');
                    });
                    $('#example').DataTable({
                        "destroy": true,
                    });

                    // remove previously loaded options

                }
            });

            $(document).ready(function () {
                $("#div_Loader").hide();
            });

            function Show() {
                $('#div_Loader').show();
            }

            function Hide() {
                $('#div_Loader').hide();
            }
        });
    });

    function deleteEmployee(id) {
            $.ajax({
                type: 'DELETE',
                url: '/api/test/'+id,
                success: function (data) {
                    console.log(data);
                    alert('Delete Successfully');
                    window.location.reload();
                }
            });
    }
    function editEmployee(id) {
        $("#submitBtn").html('Update');
        $.ajax({
            type: 'GET',
            url: '/api/test/' + id,
            dataType: 'JSON',
            success: function (data) {
                console.log(data);
                $("#id").val(data[0]["id"]);
                $("#name").val(data[0]["name"]);
                $("#email").val(data[0]["email"]);
                $("#age").val(data[0]["age"]);
                $("#designation").val(data[0]["designation"]);
            }
        });
    }

    function update() {
        var id = $("#id").val();

        var postData = {
            "name": $("#name").val(),
            "email": $("#email").val(),
            "age": $("#age").val(),
            "designation": $("#designation").val()
        }

        $.ajax({
            type: 'PUT',
            url: '/api/test/' + id,
            contentType: 'application/json',
            data: JSON.stringify(postData),
            success: function (data) {
                console.log(data);
                $("#id").val(0);
                $("#submitBtn").html('Submit');
                alert('Update Successfully');
                window.location.reload();
            }
        });
    }