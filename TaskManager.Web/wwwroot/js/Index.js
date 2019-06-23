$(() => {
    const userId = $("this").data('user-id')

    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/toDoHub").build();

    connection.start().then(() => {
        connection.invoke("GetAllToDos");
    });

    connection.on('RenderToDos', toDos => {
        $("#toDosTable tr:gt(0)").remove
        toDos.forEach(toDo => {
            let buttonHtml;
            if (toDo.toDoStatus == "Unclaimed") {
                buttonHtml = `<button class="update-status btn btn-danger btn-block" data-id="${toDo.id}" data-update="start">I'll do this one</button>`
            }
            else if (toDo.toDoStatus == "Started" && toDo.user.userId == userId) {
                buttonHtml = `<button class="update-status btn btn-success btn-block" data-id="${toDo.id}" data-update="complete">I'm done!</button>`
            }
            else {
                buttonHtml = `<button class="update-status btn btn-danger btn-block" id="started" disabled>${toDo.user.firstName} ${toDo.user.lastName} is doing this one</button>`
            }            
            $("#toDosTable").append(`<tr><td>${toDo.name}</td><td>${buttonHtml}</td></tr>`)
        });
    });

    $("#addToDo").on('click', function () {
        const name = $("#toDoName").val();
        connection.invoke("AddToDo", { name, userId});
        $("#toDoName").val('');
    });

    $("#toDosTable").on('click', ".update-status", () => {
        const Id = $(".update-status").data('id');
        const CurrentStatus = $(".update-status").data('current-status');
        let toDoStatus = "";

        if (CurrentStatus === "Unclaimed") {
            toDoStatus = "Started";
        }
        else {
            toDoStatus = "Completed";
        }

        connection.invoke("UpdateToDo", { id, toDoStatus });
    });
});