$(() => {
    const userId = $("table").data('user-id')

    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/toDoHub").build();

    connection.start().then(() => {
        connection.invoke("GetAllToDos");
    });

    connection.on('RenderToDos', toDos => {
        $("table tr:gt(0)").remove
        toDos.forEach(t => {
            let buttonHtml;
            if (t.handledBy && t.handledBy === userId) {
                buttonHtml = `<button data-task-id=${t.id} class='btn btn-success done'>I'm done!</button>`;
            } else if (t.userDoingIt) {
                buttonHtml = `<button class='btn btn-warning' disabled>${t.userDoingIt} is doing this</button>`;
            } else {
                buttonHtml = `<button data-task-id=${t.id} class='btn btn-info doing'>I'm doing this one!</button>`;
            }
            $("table").append(`<tr><td>${toDo.name}</td><td>${buttonHtml}</td></tr>`)
        });
    });

    $("#submit").on('click', function () {
        const name = $("#toDoName").val();
        connection.invoke("NewTask", name);
        $("#toDoName").val('');
    });

    $("table").on('click', '.done', function() {
        const id = $(this).data('task-id');
        connection.invoke("setDone", id);
    });

    $("table").on('click', '.doing', function () {
        const id = $(this).data('task-id');
        connection.invoke("setDoing", id);
    });
});