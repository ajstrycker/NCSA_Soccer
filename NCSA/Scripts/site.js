var loadingIcon = `<svg version="1.1" id="loadingAnim" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink"
                        width="50%" height="50%" viewBox="-40 -25 100 100" style="enable-background:new 0 0 50 50;" xml:space="preserve">
                    <rect x="0" y="10" width="4" height="10" fill="#333" opacity="0.2">
                    <animate attributeName="opacity" attributeType="XML" values="0.2; 1; .2" begin="0s" dur="0.6s" repeatCount="indefinite" />
                    <animate attributeName="height" attributeType="XML" values="10; 20; 10" begin="0s" dur="0.6s" repeatCount="indefinite" />
                    <animate attributeName="y" attributeType="XML" values="10; 5; 10" begin="0s" dur="0.6s" repeatCount="indefinite" />
                    </rect>
                    <rect x="8" y="10" width="4" height="10" fill="#333" opacity="0.2">
                    <animate attributeName="opacity" attributeType="XML" values="0.2; 1; .2" begin="0.15s" dur="0.6s" repeatCount="indefinite" />
                    <animate attributeName="height" attributeType="XML" values="10; 20; 10" begin="0.15s" dur="0.6s" repeatCount="indefinite" />
                    <animate attributeName="y" attributeType="XML" values="10; 5; 10" begin="0.15s" dur="0.6s" repeatCount="indefinite" />
                    </rect>
                    <rect x="16" y="10" width="4" height="10" fill="#333" opacity="0.2">
                    <animate attributeName="opacity" attributeType="XML" values="0.2; 1; .2" begin="0.3s" dur="0.6s" repeatCount="indefinite" />
                    <animate attributeName="height" attributeType="XML" values="10; 20; 10" begin="0.3s" dur="0.6s" repeatCount="indefinite" />
                    <animate attributeName="y" attributeType="XML" values="10; 5; 10" begin="0.3s" dur="0.6s" repeatCount="indefinite" />
                    </rect>
                </svg>`;

function SmoothScroll(id) {
    document.querySelector('#' + id).scrollIntoView({
        behavior: 'smooth'
    });

    if (document.getElementById('pfname') !== null) {
        document.getElementById('pfname').select();
    }
}

$("#dob").on('input', function (e) {
    var x;

    x = e.target.value.replace(/\D/g, '').match(/(\d{0,2})(\d{0,2})(\d{0,4})/);
    e.target.value = !x[2] ? x[1] : x[1] + '/' + (!x[2] ? '' : x[2]) + (x[3] ? '/' + x[3] : '');

    if (e.target.value.length === 10) {
        $("#dob").removeClass("form__input-invalid");
        $("#dob").addClass("form__input-valid");
    }
    else {
        $("#dob").removeClass("form__input-valid");
        $("#dob").addClass("form__input-invalid");
    }
});

$("#phone").on('input', function (e) {
    var x;

    if (e.target.value.length === 3) {
        x = e.target.value.replace(/\D/g, '').match(/(\d{0,3})/);
        e.target.value = '(' + x[1] + ') ';
    }
    else {
        x = e.target.value.replace(/\D/g, '').match(/(\d{0,3})(\d{0,3})(\d{0,4})/);
        e.target.value = !x[2] ? x[1] : '(' + x[1] + ') ' + x[2] + (x[3] ? '-' + x[3] : '');
    }

    if (e.target.value.length === 14) {
        $("#phone").removeClass("form__input-invalid");
        $("#phone").addClass("form__input-valid");
    }
    else {
        $("#phone").removeClass("form__input-valid");
        $("#phone").addClass("form__input-invalid");
    }
});


$('#approve-yes').change(() => {
    if ($('#approve-no').is(':checked')) {
        $('#approve-no').prop('checked', false);
    }
});

$('#approve-no').change(() => {
    if ($('#approve-yes').is(':checked')) {
        $('#approve-yes').prop('checked', false);
    }
});


var current_fs, next_fs, previous_fs; //fieldsets
var left, opacity, scale; //fieldset properties which we will animate
var animating; //flag to prevent quick multi-click glitches
var player = {};

$(".next").click(function () {
    if (animating) return false;

    current_fs = $(this).closest(".form__fieldset");
    next_fs = $(this).closest(".form__fieldset").next();

    current_fs.find('input[type=text]:required, select, input[type=checkbox]').each(function () {
        var closestFormGroup = $(this).closest('.form__group');

        if ($(this).attr('id') === 'dob') {
            console.log(new Date($(this).val()).getFullYear());
            if ($(this).val().length != 10 || new Date($(this).val()).getFullYear() < 1950) {
                if (closestFormGroup.find('.form__hint').length === 0) {
                    AddHint(closestFormGroup, 'Please enter a valid date of birth (MM/DD/YYYY)');
                }
            }
            else {
                if (isNaN(new Date($(this).val()))) {
                    if (closestFormGroup.find('.form__hint').length === 0) {
                        AddHint(closestFormGroup, 'Please enter a valid date of birth (MM/DD/YYYY)');
                    }
                }
                else {
                    $(this).closest('.form__group').find('.form__hint').remove();
                }
            }
        }
        else if ($(this).attr('id') === 'email') {
            const email_rg = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            if (email_rg.test(String($(this).val()).toLowerCase())) {
                $(this).closest('.form__group').find('.form__hint').remove();
            }
            else {
                if (closestFormGroup.find('.form__hint').length === 0) {
                    AddHint(closestFormGroup, 'Please enter a valid email address (ex myemail@host.com)');
                }
            }
        }
        else if ($(this).attr('id') === 'phone') {
            if ($(this).val().length != 14) {
                if (closestFormGroup.find('.form__hint').length === 0) {
                    AddHint(closestFormGroup, 'Please enter a valid phone number (574)-555-5555');
                }
            }
            else {
                $(this).closest('.form__group').find('.form__hint').remove();
            }
        }
        else if ( $(this).is('select') ) {
            if ($(this).val() === 'hide' && closestFormGroup.find('.form__hint').length === 0) {
                AddHint(closestFormGroup, 'Please Choose');
            }
            else {
                $(this).closest('.form__group').find('.form__hint').remove();
            }
        }
        else if ($(this).is('input[type=checkbox]')) {
            if ($('#approve-no').is(':checked') || $('#approve-yes').is(':checked')) {
                $('#permissionMsg').css('color', '#000');
            }
            else {
                $('#permissionMsg').animate({
                    color: '#e82a3a'
                }, 1000);
            }
        }
        else {
            if ($(this).val() === '') {

                if (closestFormGroup.find('.form__hint').length === 0) {
                    AddHint(closestFormGroup, 'This is a required field');
                }
            }
            else {
                $(this).closest('.form__group').find('.form__hint').remove();
            }
        }
    });

    if (current_fs.find('.form__hint').length !== 0) {
        AddAlert("error", "Not all fields were filled out correctly, please check the red icons for more information.", 5000);
        return;
    }

    animating = true;

    //activate next step on progressbar using the index of next_fs
    $(".form__progressbar li").eq($(".form__fieldset").index(next_fs)).addClass("active");

    if (next_fs.find(".form__review").length !== 0) {
        BuildReview();
    }
    else {
        player = {};
    }

    //show the next fieldset
    next_fs.css('visibility', 'hidden');

    //hide the current fieldset with style
    current_fs.animate({ opacity: 0 }, {
        step: function (now, mx) {
            opacity = 1 - now;

            next_fs.css({ 'opacity': opacity });
            next_fs.css('visibility', 'visible');

        },
        duration: 500,
        complete: function () {
            current_fs.css('visibility', 'hidden');
            animating = false;
        }
    });
});

$(".previous").click(function () {
    if (animating) return false;
    animating = true;

    current_fs = $(this).closest(".form__fieldset");
    previous_fs = $(this).closest(".form__fieldset").prev();

    //de-activate current step on progressbar
    $(".form__progressbar li").eq($(".form__fieldset").index(current_fs)).removeClass("active");

    //show the previous fieldset
    previous_fs.css('visibility', 'hidden');

    //hide the current fieldset with style
    current_fs.animate({ opacity: 0 }, {
        step: function (now, mx) {
            opacity = 1 - now;

            previous_fs.css({ 'opacity': opacity });
            previous_fs.css('visibility', 'visible');
        },
        duration: 500,
        complete: function () {
            current_fs.css('visibility', 'hidden');
            // remove the dynamic values from the form 
            if (current_fs.find(".form__review").length !== 0) {
                $(".form__review-item").remove();
                player = {};
            }
            animating = false;
        }
    });
});

var submittingForm = false;
$(".submit").click(function () {

    // ajax call to submit 
    if (!submittingForm) {

        submittingForm = true;

        current_fs = $(this).closest(".form__fieldset");
        current_fs.css('visibility', 'hidden');

        var loadingElem = $(".form__loading");
        loadingElem.css('visibility', 'visible');

        loadingElem.append(loadingIcon);

        $.ajax({

            url: '/home/SubmitPlayer',
            type: 'POST',
            data: {
                __RequestVerificationToken: $('input[name = "__RequestVerificationToken"]').val(),
                player: player
            },
            success: function (data) {
                // remove loading icon
                loadingElem.empty();

                // display success
                loadingElem.append(`
                <div class="form__loading-heading">
                   ${player.PlayerFirstName} has successfully signed up!  
                </div>
                <svg id="successAnimation" class="animated" xmlns="http://www.w3.org/2000/svg" width="100%" height="100%" viewBox="0 0 70 70">
                    <path id="successAnimationResult" fill="#D8D8D8" d="M35,60 C21.1928813,60 10,48.8071187 10,35 C10,21.1928813 21.1928813,10 35,10 C48.8071187,10 60,21.1928813 60,35 C60,48.8071187 48.8071187,60 35,60 Z M23.6332378,33.2260427 L22.3667622,34.7739573 L34.1433655,44.40936 L47.776114,27.6305926 L46.223886,26.3694074 L33.8566345,41.59064 L23.6332378,33.2260427 Z" />
                    <circle id="successAnimationCircle" cx="35" cy="35" r="24" stroke="#979797" stroke-width="2" stroke-linecap="round" fill="transparent" />
                    <polyline id="successAnimationCheck" stroke="#979797" stroke-width="2" points="23 34 34 43 47 27" fill="transparent" />
                </svg>`);
            },
            error: function (request, error) {
                loadingElem.css('visibility', 'hidden');
                current_fs.css('visibility', 'visible')
                AddAlert('error', JSON.parse(request.responseText).msg, 10000);
            },
            complete: function () {
                $('#loadingAnim').remove();
                submittingForm = false;
            }
        });
    }
});

function AddHint(element, message) {
    $(`<div class="hint--top hint--bounce form__hint" aria-label="${message}">
                        <i class="fa fa-info-circle"></i>
                    </div>`).appendTo(element).hide().fadeIn(500);
}

function BuildReview() {

    player.PlayerFirstName = $("#pfname").val();
    player.PlayerLastName = $("#plname").val();
    BuildElement("Name: ", player.PlayerFirstName + " " + player.PlayerLastName, 1);

    player.BirthDate = $("#dob").val();
    console.log(player.BirthDate);
    BuildElement("Date Of Birth: ", player.BirthDate, 1);

    player.Gender = $("#gender").val();
    BuildElement("Gender: ", player.Gender, 1);

    player.ShirtSize = $("#shirtSize").val();
    BuildElement("Shirt Size: ", player.ShirtSize, 1);

    player.TeamId = $('#teamId :selected').val();
    BuildElement("Team: ", $('#teamId :selected').text(), 1);

    player.ParentFirstName = $("#prfname").val();
    player.ParentLastName = $("#prlname").val();
    BuildElement("Parent Name: ", player.ParentFirstName + " " + player.ParentLastName, 2);

    player.PhoneNumber = $("#phone").val();
    BuildElement("Phone Number: ", player.PhoneNumber, 2);

    player.Email = $("#email").val();
    BuildElement("Email: ", player.Email, 2);

    player.ApprovesPictures = $("#approve").val() == 'on' ? 'y' : 'n';
    BuildElement("Approve Pictures: ", player.ApprovesPictures === 'y' ? 'Yes' : 'No', 2);

    player.Addr1 = $("#addr1").val();
    player.Addr2 = $("#addr2").val();
    BuildElement("", player.Addr1 + " " + player.Addr2, 3);

    player.City = $("#city").val();
    player.State = $("#state").val();
    player.Zip = $("#zip").val();
    BuildElement("", player.City + " " + player.State + ", " + player.Zip, 3);
}

function BuildElement(heading, val, col) {
    var element = `<div class="form__review-item">
                            <strong>${heading}</strong>
                            ${val}
                        </div>`;

    next_fs.find(".form__review-col" + col).append(element);
}

var alertAdded = false;
function AddAlert(messageType, message, timeoutTime) {

    if (alertAdded) return false;

    alertAdded = true;
    var alertText = `<input type="checkbox" id="alert" />
        <label class="close" title="close" for="alert">
            <i class="fas fa-times"></i>
        </label>
        <p class="inner">
           ${message}
        </p>`;

    $(".alerts").append(alertText);
    $(".alerts").show();
    $(".alerts").addClass(messageType + " in-out");

    setTimeout(() => {
        if ($("#alert").length > 0) {
            $(".alerts").removeClass(messageType + " in-out");
            $(".alerts input").prop("checked", true);

            setTimeout(() => {
                $(".alerts").empty();
                alertAdded = false;
            }, 300);
        }
    }, timeoutTime);
}

function Timeout(ms) {
    return () => new Promise(resolve => setTimeout(resolve, ms));
}