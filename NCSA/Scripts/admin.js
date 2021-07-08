$(".admin__teams__create-dropdown").on('change', () => {
    var townName = $(".admin__teams__create-dropdown").val(); 

    $('.admin__teams__item-town').each(function () {
        if ($(this).text().toLowerCase().trim() === townName || townName === 'all') {
            $(this).closest('.admin__teams__item').show();
        }
        else {
            $(this).closest('.admin__teams__item').hide();
        }
    });
});

function ConfirmDelete(id, pageName) {
    $('.admin').css({
        'filter': 'blur(2px)',
        'webkitFilter': 'blur(2px)',
        'mozFilter': 'blur(2px)',
        'oFilter': 'blur(2px)',
        'msFilter': 'blur(2px)',
        'transition': 'all 0.5s ease-in',
        '-webkit-transition': 'all 0.5s ease-in',
        '-moz-transition': 'all 0.5s ease-in',
        '-o-transition': 'all 0.5s ease-in'
    });

    var elem;

    if (pageName === 'schedule') {
        var game = $('#' + id);
        var homeTeam = game.find('#home_' + id).text().trim();
        var awayTeam = game.find('#away_' + id).text().trim();
        var gameDate = game.find('#date_' + id).text().trim();

        elem = `
        <div class="confirm__delete">
            <div class="confirm__delete__title" > Are you sure you want to delete this game?</div >
            <div class="confirm__delete__item mt-md">${homeTeam}</div>
            <div class="confirm__delete__item">${awayTeam}</div>
            <div class="confirm__delete__item">${gameDate}</div>
            <div class="confirm__delete__btns">
                <input type="button" class="bttn bttn-secondary" value="Yes, Delete" onclick="DoDelete(${id}, 'ScheduleDelete' )" />
                <input type="button" class="bttn bttn-secondary ml-sm" value="Cancel" onclick="CancelDelete()" />
            </div>
         </div >`;
    }
    else if (pageName === 'user') {
        var user = $('#' + id);
        var username = user.find('#un_' + id).text().trim();

        elem = `
        <div class="confirm__delete">
            <div class="confirm__delete__title" > Are you sure you want to delete this user?</div >
            <div class="confirm__delete__item mt-md mb-md">${username}</div>
            <div class="confirm__delete__btns">
                <input type="button" class="bttn bttn-secondary" value="Yes, Delete" onclick="DoDelete('${id}', 'UserDelete' )" />
                <input type="button" class="bttn bttn-secondary ml-sm" value="Cancel" onclick="CancelDelete()" />
            </div>
         </div >`;
    }
    else if (pageName === 'player') {
        var player = $('#' + id);
        var name = player.find('#pname_' + id).text().trim();
        var desc = player.find('#pdesc_' + id).text().trim();

        elem = `
        <div class="confirm__delete">
            <div class="confirm__delete__title" > Are you sure you want to delete this player?</div >
            <div class="confirm__delete__title mt-md">${name}</div>
            <div class="confirm__delete__item">${desc}</div>
            <div class="confirm__delete__btns">
                <input type="button" class="bttn bttn-secondary" value="Yes, Delete" onclick="DoDelete('${id}', 'PlayerDelete' )" />
                <input type="button" class="bttn bttn-secondary ml-sm" value="Cancel" onclick="CancelDelete()" />
            </div>
         </div >`;
    }
    else if (pageName === 'team') {

        $.ajax({
            url: '/admin/IsTeamIdInUse/' + id,
            type: 'GET',
            success: function (data) {
                if (data.toLowerCase() === 'true') {
                    AddAlert("error", "Delete all games associated with this team before deleting this team.", 10000)
                    CancelDelete();
                }
                else {
                    var team = $('#' + id);
                    var town = team.find('.admin__teams__item-town').text().trim();
                    var desc = team.find('.admin__teams__item-description').text().trim();
                    var coach = team.find('.coach').text().trim().replace(/(\r\n|\n|\r)/gm, "").replace(/  +/g, ' ');

                    elem = `
                    <div class="confirm__delete">
                        <div class="confirm__delete__title" > Are you sure you want to delete this Team?</div >
                        <div class="confirm__delete__team">${town} - ${desc}</div>
                        <div class="confirm__delete__coach">${coach}</div>
                        <div class="confirm__delete__btns">
                            <input type="button" class="bttn bttn-secondary" value="Yes, Delete" onclick="DoDelete(${id}, 'TeamsDelete')" />
                            <input type="button" class="bttn bttn-secondary ml-sm" value="Cancel" onclick="CancelDelete()" />
                        </div>
                     </div >`;
                }
            },
            error: function () {
                AddAlert('error', 'There was a problem removing the item. Check the error logs for more details.', 10000);
            },
            complete: function () {
                $('body').append(elem);
                $('.confirm__delete').fadeIn(500).css('display', 'flex');
            }
        });
    }
    else if (pageName === 'location') {
        $.ajax({
            url: '/admin/IsLocationIdInUse/' + id,
            type: 'GET',
            success: function (data) {
                if (data.toLowerCase() === 'true') {
                    AddAlert("error", "Delete all teams associated with this location before deleting this location.", 10000)
                    CancelDelete();
                }
                else {
                    var game = $('#' + id);
                    var address = game.find('#addr_' + id).text().trim();
                    var citystatezip = game.find('#citystzip_' + id).text().trim();
                    var longitude_latitude = game.find('#longlat' + id).text().trim();

                    elem = `
                    <div class="confirm__delete">
                        <div class="confirm__delete__title" > Are you sure you want to delete this game?</div >
                        <div class="confirm__delete__item mt-md">${address}</div>
                        <div class="confirm__delete__item">${citystatezip}</div>
                        <div class="confirm__delete__item">${longitude_latitude}</div>
                        <div class="confirm__delete__btns">
                            <input type="button" class="bttn bttn-secondary" value="Yes, Delete" onclick="DoDelete(${id}, 'LocationDelete' )" />
                            <input type="button" class="bttn bttn-secondary ml-sm" value="Cancel" onclick="CancelDelete()" />
                        </div>
                     </div >`;
                }
            },
            error: function () {
                AddAlert('error', 'There was a problem removing the item. Check the error logs for more details.', 10000);
            },
            complete: function () {
                $('body').append(elem);
                $('.confirm__delete').fadeIn(500).css('display', 'flex');
            }
        });
    }

    $('body').append(elem);
    $('.confirm__delete').fadeIn(500).css('display', 'flex');
}

function CancelDelete() {
    $('.confirm__delete').fadeOut(500, function () {
        $(this).remove();
    });

    $('.admin').css({
        'filter': 'blur(0px)',
        'webkitFilter': 'blur(0px)',
        'mozFilter': 'blur(0px)',
        'oFilter': 'blur(0px)',
        'msFilter': 'blur(0px)',
        'transition': 'all 0.5s ease-out',
        '-webkit-transition': 'all 0.5s ease-out',
        '-moz-transition': 'all 0.5s ease-out',
        '-o-transition': 'all 0.5s ease-out'
    });
}

function DoDelete(id, func) {
    $.ajax({
        url: '/admin/' + func + '/' + id,
        type: 'DELETE',
        data: {
            __RequestVerificationToken: $('input[name = "__RequestVerificationToken"]').val(),
            id: id
        },
        success: function () {
            $('#' + id).fadeOut(500, function () {
                $(this).remove();
            });

            AddAlert('success', 'Successfully Deleted!', 3000)
        },
        error: function () {
            AddAlert('error', 'There was a problem removing the item. Check the error logs for more details.', 10000);
        },
        complete: function () {
            CancelDelete();
        }
    });
}

function UnlockUser(userId) {
    $.ajax({
        url: '/admin/UnlockUser',
        type: 'POST',
        data: {
            __RequestVerificationToken: $('input[name = "__RequestVerificationToken"]').val(),
            userId: userId
        },
        success: function () {
            $('#lock_' + userId).fadeOut(500, function () {
                $(this).remove();
            });
        },
        error: function () {
            AddAlert('error', 'There was a problem unlocking the user. Check the error logs for more details.', 10000);
        }
    });
}

function RefereeSelect(gameId) {
    $('.admin').css({
        'filter': 'blur(2px)',
        'webkitFilter': 'blur(2px)',
        'mozFilter': 'blur(2px)',
        'oFilter': 'blur(2px)',
        'msFilter': 'blur(2px)',
        'transition': 'all 0.5s ease-in',
        '-webkit-transition': 'all 0.5s ease-in',
        '-moz-transition': 'all 0.5s ease-in',
        '-o-transition': 'all 0.5s ease-in'
    });

    var game = $('#open_' + gameId);
    var gameDate = game.find('#udate_' + gameId).text().trim();
    var gameDesc = game.find('#udesc_' + gameId).text().trim();
    var center = game.find('#ucenter_' + gameId).text().replace('Center:', '').trim();
    var ar1 = game.find('#uar1_' + gameId).text().replace('AR1:', '').trim();
    var ar2 = game.find('#uar2_' + gameId).text().replace('AR2:', '').trim();

    var elem = `
        <div class="confirm__delete">

            <div class="confirm__delete__title" >What position to you want to be assigned to?</div >
            <div class="confirm__delete__title mt-md">${gameDate}</div>
            <div class="confirm__delete__title">${gameDesc}</div>
            <ul class="confirm__delete__list">`;
    if (center.trim() === '') {
        elem += `<li class="confirm__delete__list-li">Center: <a class="bttn bttn-link" onclick="AssignToGame('${gameId}', 'center');">Assign Me Center</a></li>`;
    }
    else {
        elem += `<li class="confirm__delete__list-li">Center: ${center}</li>`;
    }

    if (ar1.trim() === '') {
        elem += `<li class="confirm__delete__list-li">AR1: <a class="bttn bttn-link" onclick="AssignToGame('${gameId}', 'ar1');">Assign Me AR1</a></li>`;
    }
    else {
        elem += `<li class="confirm__delete__list-li">AR1: ${ar1}</li>`;
    }

    if (ar2.trim() === '') {
        elem += `<li class="confirm__delete__list-li">AR2: <a class="bttn bttn-link" onclick="AssignToGame('${gameId}', 'ar2');">Assign Me AR2</a></li>`;
    }
    else {
        elem += `<li class="confirm__delete__list-li">AR2: ${ar2}</li>`;
    }

    elem += `
            </ul>
            <div class="confirm__delete__btns">
                <input type="button" class="bttn bttn-secondary ml-sm" value="Cancel" onclick="CancelDelete()" />
            </div>

            <div class="form__loading">

            </div>
         </div >`;

    $('body').append(elem);
    $('.confirm__delete').fadeIn(500).css('display', 'flex');
}

function AssignToGame(gameId, pos) {
    console.log('Assigning...');
    var loadingElem = $(".form__loading");
    loadingElem.css('visibility', 'visible');

    loadingElem.append(loadingIcon);

    $.ajax({

        url: '/admin/AddRefToGame',
        type: 'POST',
        data: {
            __RequestVerificationToken: $('input[name = "__RequestVerificationToken"]').val(),
            gameID: gameId,
            pos: pos
        },
        success: function (data) {
            // remove loading icon
            loadingElem.empty();

            // remove the modal from the screen 
            CancelDelete();

            AddAlert('success', 'You have been successfully added to the game.', 10000);

            var game = $('#open_' + gameId);

            if (pos === 'center') {
                $('#ucenter_' + gameId).empty();
                $('#ucenter_' + gameId).append(`<span>Center Ref:</span> ${JSON.parse(data).name}`)
            }
            else if (pos === 'ar1') {
                $('#uar1_' + gameId).empty();
                $('#uar1_' + gameId).append(`<span>AR1:</span> ${JSON.parse(data).name}`)
            }
            else if (pos === 'ar2') {
                $('#uar2_' + gameId).empty();
                $('#uar2_' + gameId).append(`<span>AR2:</span> ${JSON.parse(data).name}`)
            }

            $('#btn_' + gameId).remove();

            game.fadeOut(250, function () {
                game.clone().appendTo('#assigned').hide().fadeIn(250);
            });
        },
        error: function (request, error) {
            loadingElem.css('visibility', 'hidden');
            AddAlert('error', JSON.parse(request.responseText).msg, 10000);
        },
        complete: function () {
            $('#loadingAnim').remove();
        }
    });
}

$("#gradesFilter").on('change', () => {
    var grade = $("#gradesFilter").val();
    $('#playersearch').val('');
    $('.pdesc').each(function () {
        var desc = $(this).text().replace('Team:', '').trim();

        if (desc === grade || grade === 'all') {
            $(this).closest('.admin__container__item').show();
        }
        else {
            $(this).closest('.admin__container__item').hide();
        }
    });
});

$("#playersearch").on('input', () => {

    var searchText = $('#playersearch').val();
    $("#gradesFilter>option:eq(0)").prop('selected', true);

    $('.admin__container__item').each(function () {
        if (searchText.trim() === "") {
            $(this).show();
        }
        else {
            var name = $(this).find('.pname').text().replace('Name:', '').trim();
            var desc = $(this).find('.pdesc').text().replace('Team:', '').trim();
            var phone = $(this).find('.pphone').text().replace('Phone:', '').trim();
            var email = $(this).find('.pemail').text().replace('Email:', '').trim();

            if (name.indexOf(searchText) != -1 || desc.indexOf(searchText) != -1 || phone.indexOf(searchText) != -1 || email.indexOf(searchText) != -1) {
                $(this).show();
            }
            else {
                $(this).hide();
            }
        }
    });
});