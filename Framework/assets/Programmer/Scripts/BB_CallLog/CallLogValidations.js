
//Request Finanace Validation
$('#btnrequestfinanceform').validate({
    errorElement: 'span',
    errorClass: 'help-block help-block-error',
    rules: {
        Purpose: "required",
        ddlProductId: "required",
        RequiredOn: "required",
        Tenure: "required",
        Amount: "required",
        PaymentDetails: "required"
    },
    highlight: function (element) {
        // hightlight error inputs
        $(element)
            .closest('.form-group').removeClass('has-success').addClass('has-error'); // set error class to the control group
    },
    unhighlight: function (element) { // revert the change done by hightlight
        $(element)
            .closest('.form-group').removeClass('has-error'); // set error class to the control group
    },
    submitHandler: function (form) {
        form.submit();
    }

});



//General Discussion Validation
$('#btgeneraldiscussionform').validate({
    errorElement: 'span',
    errorClass: 'help-block help-block-error',
    rules: {
        GTitle: "required",
        GeneralDiscussion: "required"
    },
    highlight: function (element) {
        // hightlight error inputs
        $(element)
            .closest('.form-group').removeClass('has-success').addClass('has-error'); // set error class to the control group
    },
    unhighlight: function (element) { // revert the change done by hightlight
        $(element)
            .closest('.form-group').removeClass('has-error'); // set error class to the control group
    },
    submitHandler: function (form) {
        form.submit();
    }

});

//Action Point Validation
$('#btnactionpointform').validate({
    errorElement: 'span',
    errorClass: 'help-block help-block-error',
    rules: {
        TaskTitle: "required",
        Description: "required",ADescription:"required",
        DueDate: "required",
        Tags: "required"

    },
    highlight: function (element) {
        // hightlight error inputs
        $(element)
            .closest('.form-group').removeClass('has-success').addClass('has-error'); // set error class to the control group
    },
    unhighlight: function (element) { // revert the change done by hightlight
        $(element)
            .closest('.form-group').removeClass('has-error'); // set error class to the control group
    },
    submitHandler: function (form) {
        form.submit();
    }

});

