$(() => {
    const userId = $("table").data('user-id')

    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/toDoHub").build();

    connection.start().then(() => {
        connection.invoke("GetAll");
    });

    connection.on('RenderTasks', tasks => {
        $("table tr:gt(0)").remove();
        tasks.forEach(t => {
            let buttonHtml;
            if (t.handledBy && t.handledBy === userId) {
                buttonHtml = `<button data-task-id=${t.id} class='btn btn-success done'>I'm done!</button>`;
            } else if (t.userDoingIt) {
                buttonHtml = `<button class='btn btn-warning' disabled>${t.userDoingIt} is doing this</button>`;
            } else {
                buttonHtml = `<button data-task-id=${t.id} class='btn btn-info doing'>I'm doing this one!</button>`;
            }
            $("table").append(`<tr><td>${t.title}</td><td>${buttonHtml}</td></tr>`)
        });
    });

    $("#submit").on('click', function () {
        const title = $("#title").val();
        connection.invoke("NewToDo", title);
        $("#title").val('');
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