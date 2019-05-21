$(() => {

    clearAndPopulateTable();

    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/toDoHub").build();

    connection.on("addToDoSignalR", () => {
        clearAndPopulateTable();
    });

    connection.on("UpdateToDo", () => {
        clearAndPopulateTable();
    });

    $("#addToDo").on('click', () => {
        const name = $("#toDoName").val();
        $.post("/home/addToDo", { name }, function () {
        });
        connection.invoke("AddToDoSignalR", { name });
    });

    connection.on("NewToDo", ToDo => {
        $("#toDosTable").append(
            `<tr>
                    <td>${ToDo.name}</td>
                    <td>
                       <button class="update-status btn btn-danger btn-block" data-id="${ToDo.id}" data-update="start">I'll do this one</button>
                    </td>
                </tr>`);
    });
    
    $(".update-status").on('click', () => {
        const Id = $(".update-status").data('id');
        const CurrentStatus = $(".update-status").data('current-status');
        let toDoStatus = "";

        if (CurrentStatus === "Unclaimed") {
            toDoStatus = "Started";
        }
        else {
            toDoStatus = "Completed";
        }

        $.post("/home/updateToDo", { Id, toDoStatus }, function () {
            clearAndPopulateTable();

            //    if (CurrentStatus === "Unclaimed") {
            //        console.log('started')
            //        previouslyClicked.removeClass("btn-tab-success").addClass("btn-warning");  
            //    }
            //    else {
            //        toDoStatus = "Completed";
            //    }

        });
        connection.invoke("UpdateToDo");
    });

    function clearAndPopulateTable() {
        $("#toDosTable tr:gt(0)").remove();
        $.get('/home/getIncompleteToDos', function (result) {
            result.forEach(ToDo => {
                if (ToDo.toDoStatus == "Unclaimed") {
                    $("#toDosTable").append(
                        `<tr>
                            <td>${ToDo.name}</td>
                            <td>                                
                                <button class="update-status btn btn-danger btn-block" data-id="${ToDo.id}" data-update="start">I'll do this one</button>
                            </td >
                        </tr >`);
                }
                else if (ToDo.toDoStatus == "Started" && ToDo.user.email == '@Context.User.Identity.Name') {
                    $("#toDosTable").append(
                        `<tr>
                            <td>${ToDo.name}</td>
                            <td>                                
                                <button class="update-status btn btn-success btn-block" data-id="${ToDo.id}" data-update="complete">I'm done!</button>
                            </td >
                        </tr >`);
                }
                else {
                    $("#toDosTable").append(
                        `<tr>
                            <td>${ToDo.name}</td>
                            <td>                                
                                <button class="update-status btn btn-danger btn-block" id="started" disabled>${ToDo.user.firstName} ${ToDo.user.lastName} is doing this one</button>
                            </td >
                        </tr >`);
                }
            });
        });
    }
});