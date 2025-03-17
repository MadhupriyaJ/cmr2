var FormWizard = function () {


    return {
        //main function to initiate the module
        init: function () {
            if (!jQuery().bootstrapWizard) {
                return;
            }

            var form = $('#submit_form');
            var error = $('.alert-danger', form);
            var success = $('.alert-success', form);

            $('.date-picker').datepicker({
                format: 'yyyy/mm/dd',
                rtl: Metronic.isRTL(),
                orientation: "left",
                autoclose: true

            });


            $('body').on('click', 'a.btn.default.btn-xs.black', function (e) {
                var link = $(this).attr("href"); // "get" the intended link in a var
                e.preventDefault();
                bootbox.confirm("Are you sure, You want to delete this row?", function (result) {
                    if (result) {
                        document.location.href = link;  // if result, "set" the document location
                    }
                });
            });

            form.validate({
                doNotHideMessage: true, //this option enables to show the error/success messages on tab switch.
                errorElement: 'span', //default input error message container
                errorClass: 'help-block help-block-error', // default input error message class
                focusInvalid: false, // do not focus the last invalid input
                rules: {
                    RPnumber: {
                        required: true
                    },
                    Passport: {
                        required: true
                    },
                    PersonModeId: {
                        required: true
                    },
                    Salutation: {
                        required: true
                    },
                    FirstName: {
                        required: true
                    },
                    LastName: {
                        required: true
                    },
                    Language: {
                        required: true
                    }
                },

                messages: { // custom messages for radio buttons and checkboxes
                    'payment[]': {
                        required: "Please select at least one option",
                        minlength: jQuery.validator.format("Please select at least one option")
                    }
                },

                errorPlacement: function (error, element) { // render error placement for each input type
                    if (element.attr("name") == "gender") { // for uniform radio buttons, insert the after the given container
                        error.insertAfter("#form_gender_error");
                    } else if (element.attr("name") == "payment[]") { // for uniform checkboxes, insert the after the given container
                        error.insertAfter("#form_payment_error");
                    } else {
                        error.insertAfter(element); // for other inputs, just perform default behavior
                    }
                },

                invalidHandler: function (event, validator) { //display error alert on form submit   
                    success.hide();
                    error.show();
                    Metronic.scrollTo(error, -200);
                },

                highlight: function (element) { // hightlight error inputs
                    $(element)
                        .closest('.form-group').removeClass('has-success').addClass('has-error'); // set error class to the control group
                },

                unhighlight: function (element) { // revert the change done by hightlight
                    $(element)
                        .closest('.form-group').removeClass('has-error'); // set error class to the control group
                },

                success: function (label) {
                    if (label.attr("for") == "gender" || label.attr("for") == "payment[]") { // for checkboxes and radio buttons, no need to show OK icon
                        label
                            .closest('.form-group').removeClass('has-error').addClass('has-success');
                        label.remove(); // remove error label here
                    } else { // display success icon for other inputs
                        label
                            .addClass('valid') // mark the current input as valid and display OK icon
                        .closest('.form-group').removeClass('has-error').addClass('has-success'); // set success class to the control group
                    }
                },

                submitHandler: function (form) {
                    success.show();
                    error.hide();
                    //add here some ajax code to submit your form or just call form.submit() if you want to submit the form without ajax
                }

            });

            var displayConfirm = function () {
                $('#tab5 .form-control-static', form).each(function () {
                    var input = $('[name="' + $(this).attr("data-display") + '"]', form);
                    if (input.is(":radio")) {
                        input = $('[name="' + $(this).attr("data-display") + '"]:checked', form);
                    }
                    if (input.is(":text") || input.is("textarea")) {
                        $(this).html(input.val());
                    } else if (input.is("select")) {
                        $(this).html(input.find('option:selected').text());
                    } else if (input.is(":radio") && input.is(":checked")) {
                        $(this).html(input.attr("data-title"));
                    } else if ($(this).attr("data-display") == 'payment[]') {
                        var payment = [];
                        $('[name="payment[]"]:checked', form).each(function () {
                            payment.push($(this).attr('data-title'));
                        });
                        $(this).html(payment.join("<br>"));
                    }
                });
            }

            var handleTitle = function (tab, navigation, index) {
                var total = navigation.find('li').length;
                var current = index + 1;
                // set wizard title
                $('.step-title', $('#form_wizard_1')).text('Step ' + (index + 1) + ' of ' + total);
                // set done steps
                jQuery('li', $('#form_wizard_1')).removeClass("done");
                var li_list = navigation.find('li');
                for (var i = 0; i < index; i++) {
                    jQuery(li_list[i]).addClass("done");
                }

                if (current == 1) {
                    $('#form_wizard_1').find('.button-previous').hide();
                } else {
                    $('#form_wizard_1').find('.button-previous').show();
                }

                if (current >= total) {
                    $('#form_wizard_1').find('.button-next').hide();
                    $('#form_wizard_1').find('.button-submit').show();
                    displayConfirm();
                } else {
                    $('#form_wizard_1').find('.button-next').show();
                    $('#form_wizard_1').find('.button-submit').hide();
                }
                Metronic.scrollTo($('.page-title'));
            }

            // default form wizard
            $('#form_wizard_1').bootstrapWizard({
                'nextSelector': '.button-next',
                'previousSelector': '.button-previous',
                onTabClick: function (tab, navigation, index, clickedIndex) {
                    return false;
                    /*
                    success.hide();
                    error.hide();
                    if (form.valid() == false) {
                        return false;
                    }
                    handleTitle(tab, navigation, clickedIndex);
                    */
                },
                onNext: function (tab, navigation, index) {
                    success.hide();
                    error.hide();

                    if (form.valid() == false) {
                        return false;
                    }

                    handleTitle(tab, navigation, index);
                },
                onPrevious: function (tab, navigation, index) {
                    success.hide();
                    error.hide();

                    handleTitle(tab, navigation, index);
                },
                onTabShow: function (tab, navigation, index) {
                    var total = navigation.find('li').length;
                    var current = index + 1;
                    var $percent = (current / total) * 100;
                    $('#form_wizard_1').find('.progress-bar').css({
                        width: $percent + '%'
                    });
                }
            });

            $('#form_wizard_1').find('.button-previous').hide();
            $('#form_wizard_1 .button-submit').click(function () {

                var Company;

                var PersonData = {
                    'RPnumber': $('#RPnumber').val(),
                    'Passport': $('#Passport').val(),
                    'PersonModeId': $('select[name="PersonModeId"] option:selected').val(),
                    'SalutationId': $('select[name="Salutation"] option:selected').val(),
                    'FirstName': $('#FirstName').val(),
                    'MiddleName': $('#MiddleName').val(),
                    'LastName': $('#LastName').val(),
                    'NickName': $('#NickName').val(),
                    'GenderId': $('select[name="Gender"] option:selected').val(),
                    'Dob': $('#DOB').val(),
                    'Mobile': $('#Mobile').val(),
                    'Phone': $('#Phone').val(),
                    'Email': $('#Email').val(),
                    'Nationality': $('select[name="Nationality"] option:selected').val(),
                    'LanguageId': $('select[name="Language"] option:selected').val(),
                    'Country': $('select[name="Country"] option:selected').val(),
                    'State_province': $('select[name="State"] option:selected').val(),
                    'City': $('select[name="City"] option:selected').val(),
                    'Street': $('#Street').val(),
                    'ZipPostalcode': $('#ZipPostalcode').val(),
                    'Occupation': $('select[name="Occupation"] option:selected').val(),
                    'Company': $('select[name="Company"] option:selected').val()

                };

                var PersonCompanyData = {
                    'Name': $('#Mcompany').val(),
                    'CompanyMode': $('select[name="MCompanyMode"] option:selected').val(),
                    'CountryId': $('select[name="MCountry"] option:selected').val(),
                    'StateId': $('select[name="MState"] option:selected').val(),
                    'CityId': $('select[name="MCity"] option:selected').val(),
                    'Street': $('#MStreet').val(),
                    'ZipCode': $('#MZipCode').val()
                };


                if ($("#companyislisted").prop('checked') == true) {
                    alert("Checked");
                    $.ajax({
                        type: 'POST',
                        dataType: 'json',
                        url: '../../PersonCompany/AddPersonCompany',
                        data: ({ PersonCompany: PersonCompanyData }),
                        success: function (response, textStatus, jqXHR) {
                            if (response != null) {
                                alert(response.ID);
                                Company = response.ID;
                                $.ajax({
                                    type: 'POST',
                                    url: '../../PersonManagment/AddPerson',
                                    data: ({ persontbl: PersonData, Location: $('[id*=Location] option:selected').text(), CompanyExternal: Company }),
                                    success: function (response) {
                                        if (response == 'True') {
                                            bootbox.alert("Person Added Successfully!", function () {
                                                window.location.href = '/PersonManagment/Index';
                                            });
                                        }
                                        else {
                                            bootbox.alert("Error, could not complete the action!", function () {
                                                window.location.reload();
                                            });

                                        }
                                    }

                                });

                            }
                            else {
                                bootbox.alert("Error, could not complete the action!", function () {
                                    window.location.reload();
                                });

                            }
                        }
                    });
                }

                else {
                    $.ajax({
                        type: 'POST',
                        url: '../../PersonManagment/AddPerson',
                        data: ({ persontbl: PersonData, Location: $('[id*=Location] option:selected').text(), CompanyExternal: Company }),
                        success: function (response) {
                            if (response == 'True') {
                                bootbox.alert("Person Added Successfully!", function () {
                                    window.location.href = '/PersonManagment/Index';
                                });
                            }
                            else {
                                bootbox.alert("Error, could not complete the action!", function () {
                                    window.location.reload();
                                });

                            }
                        }

                    });


                }






            }).hide();


        }

    };

}();