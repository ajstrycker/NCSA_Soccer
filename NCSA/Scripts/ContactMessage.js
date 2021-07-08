var submittingForm = false;
$("#submit").click(function () {
    var message = {};
    message.Name = $("#ContactName").val();
    message.Email = $("#ContactEmail").val();
    message.Message = $("#ContactMessage").val();

    if (!submittingForm) {
        submittingForm = true;

        $("#ContactName").prop('disabled', true);
        $("#ContactEmail").prop('disabled', true);
        $("#ContactMessage").prop('disabled', true);
        $("#submit").prop('disabled', true);

        $.ajax({

            url: '/home/AddMessage',
            type: 'POST',
            data: {
                __RequestVerificationToken: $('input[name = "__RequestVerificationToken"]').val(),
                Message: message
            },
            success: function (data) {
                AddAlert('success', 'The message was sent! We will respond to the email provided. ', 60000);

                $("#ContactName").val("");
                $("#ContactEmail").val("");
                $("#ContactMessage").val("");
                $("#submit").val("");
            },
            complete: function () {
                submittingForm = false;

                $("#ContactName").prop('disabled', false);
                $("#ContactEmail").prop('disabled', false);
                $("#ContactMessage").prop('disabled', false);
                $("#submit").prop('disabled', false);
            }
        });
    }
});