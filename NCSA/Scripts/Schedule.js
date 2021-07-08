$(document).ready(() => {

    // get this month and make it the active calendar
    activeMonth = new Date().getMonth();
    $(`.schedule .schedule__calendar:eq(${activeMonth})`).removeClass('hideCalendar');

    if (activeMonth === 0) {
        hidingButton = true;
        $('.schedule-arrowleft').fadeOut(200, () => {
            hidingButton = false;
        });
    }
    else if (activeMonth === 11) {
        hidingButton = true;
        $('.schedule-arrowright').fadeOut(200, () => {
            hidingButton = false;
        });
    }

    if ($(document).width() <= 600) {
        for (let game of $('.game')) {
            game.innerText = "";
        }

        var baseDate = new Date(Date.UTC(2017, 0, 2)); // just get a Sunday
        for (let head of $('.schedule__calendar-head')) {
            head.innerText = baseDate.toLocaleDateString('en-US', { weekday: 'short' });
            baseDate.setDate(baseDate.getDate() + 1);
        }
    }
    else if ($(document).width() < 1200) {

        for (let game of $('.game')) {
            game.innerText = game.innerText.substr(0, 3);
        }
    }
});

var gamesForDay;
var gettingGames = false;
var activeMonth;

function GoToDay(date, element) {
    gettingGames = true;

    // add loading animation until games are received
    var scheduleElem = $(".schedule");
    scheduleElem.append(`
            <div class="schedule__loading">
                ${loadingIcon}
            </div>`);

    if ($('.schedule__singleday').length !== 0)
    {
        $('.schedule__singleday').remove();
    }

    $.ajax({

        url: '/home/GetGame',
        type: 'GET',
        data: {
            'date': JSON.stringify(date)
        },
        success: function (data) {
            gamesForDay = JSON.parse(data);

            var month = new Date(date).getMonth();
            $(`.schedule .schedule__calendar:eq(${month})`).addClass('hideCalendar');
            $('.schedule-arrowright').fadeOut(250);
            $('.schedule-arrowleft').fadeOut(250);

            var singleDayMarkup = `<div class="schedule__singleday">
                                    <div class="schedule__singleday__game">
                                        <div class="schedule__singleday__game-date">
                                            ${moment(Date.parse(date)).format('MMMM DD, YYYY')} 
                                            <i class="fa fa-calendar-alt ml-sm schedule__singleday__game-calendar" title="Calendar View" onclick="ShowCalendar();"></i>
                                        </div>
                                        <i class="fa fa-arrow-alt-circle-left schedule__singleday__game-arrowleft" onclick="ChangeDay('back', '${date}');"></i>
                                        <i class="fa fa-arrow-alt-circle-right schedule__singleday__game-arrowright" onclick="ChangeDay('forward', '${date}');"></i>`;

            if (gamesForDay.length === 0) {
                singleDayMarkup += `
                        <div class="schedule__singleday__game-text">
                            There are no games scheduled for this day. 
                        </div>`;
            }
            else {
                singleDayMarkup += `<div class="schedule__singleday__game-time"><span>8:00 AM</span></div>`;
                var parsedDate = moment(Date.parse(date)).format('YYYY-MM-DD');
                var dayTime = moment(parsedDate + 'T08:00:00');

                $.each(gamesForDay, (i, item) => {
                    var gameTime = moment(item.GameDateTime);
                    var timeFound = false;

                    while (!timeFound) {
                        var dateToCheck = dayTime.clone(); // have to clone since .add will change the value when we only want to check
                        if (dayTime >= gameTime && gameTime < dateToCheck.add(1, 'hours')) {
                            timeFound = true;
                        }
                        else {
                            dayTime = dayTime.add(1, 'hours')
                            singleDayMarkup += `<div class="schedule__singleday__game-time"><span>${moment(dayTime).format('h:00 A')}</span></div>`;
                        }
                    }
                    singleDayMarkup += `<div class="schedule__singleday__game-game ${item.HomeTeamName.toLowerCase()}-partial"  onclick="ShowLocation('${item.GameID}');">${item.HomeTeamDesc} @ ${item.HomeTeamName} vs ${item.AwayTeamName} - ${item.GameTime}</div>`;
                });
            }

            singleDayMarkup += '</div>';
            $('.schedule__loading').remove();
            $('.schedule').append(singleDayMarkup);
            $('.schedule__singleday').hide().fadeIn(250);
        },
        error: function (request, error) {
            console.log(error);
            AddAlert('error', 'There was an error getting the games. If the issue persists please contact us but using the contact page of the site.', 10000);
        },
        complete: function () {
            if ($('.schedule__loading').length > 0) {
                $('.schedule__loading').remove();
            }
            gettingGames = false;
        }
    });
}

// Used by the arrow buttons when using calendar to either go forward or backward a month
var hidingButton = false;
function ChangeMonth(forwardOrBack) {
    if (hidingButton) {
        return;
    }

    if ((activeMonth - 1) === 0 && forwardOrBack === "back") {
        hidingButton = true;
        $('.schedule-arrowleft').fadeOut(200, () => {
            hidingButton = false;
        });
    }
    else if ((activeMonth + 1 === 11) && forwardOrBack === "forward") {
        hidingButton = true;
        $('.schedule-arrowright').fadeOut(200, () => {
            hidingButton = false;
        });
    }
    else {
        if ($('.schedule-arrowleft').is(':hidden')) {
            $('.schedule-arrowleft').fadeIn(250);
        }

        if ($('.schedule-arrowright').is(':hidden')) {
            $('.schedule-arrowright').fadeIn(250);
        }
    }

    $(`.schedule .schedule__calendar:eq(${activeMonth})  `).addClass('hideCalendar');

    if (forwardOrBack === "back") {
        $(`.schedule .schedule__calendar:eq(${activeMonth - 1})`).removeClass('hideCalendar');
        activeMonth--;
    }
    else if (forwardOrBack === "forward") {
        $(`.schedule .schedule__calendar:eq(${activeMonth + 1})`).removeClass('hideCalendar');
        activeMonth++;
    }
}

// using arrow keys when looking at a single day of games
function ChangeDay(forwardOrBack, currentDate) {
    if (forwardOrBack === 'back') {
        var newDate = moment(currentDate).add(-1, 'days');
        GoToDay(newDate);
    }
    else if (forwardOrBack === 'forward') {
        var newDate = moment(currentDate).add(1, 'days');
        GoToDay(newDate);
    }
}

// when the user is in a single day view, they can switch back to calendar view 
function ShowCalendar() {
    $('.schedule__singleday').remove();
    $(`.schedule .schedule__calendar:eq(${activeMonth})  `).removeClass('hideCalendar');

    if ((activeMonth) > 0) {
        $('.schedule-arrowleft').fadeIn(250);
    }

    if ((activeMonth ) < 11) {
        $('.schedule-arrowright').fadeIn(250);
    }
}

async function ShowLocation(GameID) {
    // if there is not already a location showing then display the location
    if ($('.schedule__singleday__location').length === 0) {

    }



    var game = gamesForDay.find(item => {
        if (item.GameID.toString() === GameID.toString()) {
            return item;
        }
    });

    if ($('.schedule__singleday__location').length !== 0) {
        await RemoveLocation();
    }

    var $loc =
        $(`
        <div class="schedule__singleday__location popin" id="singleday">        
            <span class="schedule__singleday__location-removebtn"><i class="fa fa-times" onclick="RemoveLocation();"></i></span>
            <div class="schedule__singleday__location-head">${game.HomeTeamName} - ${game.HomeTeamDesc}</div>
            <div class="schedule__singleday__location-title">${game.GameDate} - ${game.GameTime}</div>
            <hr />
            <div class="schedule__singleday__location-detail"><b>Home Team:</b> ${game.HomeTeamName}</div>
            <div class="schedule__singleday__location-detail mb-lg"><b>Away Team:</b> ${game.AwayTeamName}</div>
            <div class="schedule__singleday__location-title">Game Location</div>
            <hr />
            <div class="schedule__singleday__location-detail">${game.Location.StreetName}</div>
            <div class="schedule__singleday__location-detail">${game.Location.City + ' ' + game.Location.State + ', ' + game.Location.Zip}</div>
            <a href="${GetUrl(game)}" class="schedule__singleday__location-link" target="_blank">Click To View on Map <i class="fa fa-map"></i></a>
        </div>`).hide().addClass('transReset');

    $('.schedule__singleday').append($loc);
    $loc.show(300, () => {
        $('.schedule__singleday__location').removeClass('transReset');

        SmoothScroll('singleday');
        // if using google maps api
        //var uluru = { lat: parseFloat(game.Location.Latitude), lng: parseFloat(game.Location.Longitude) };
        //var map = new google.maps.Map(document.getElementById('gmap'), { zoom: 14, center: uluru });
        //var marker = new google.maps.Marker({ position: uluru, map: map });
    });  
}

function GetUrl(game) {
    var ua = navigator.userAgent.toLowerCase(),
        plat = navigator.platform,
        protocol = '';
    var device = ua.match(/android|webos|iphone|ipad|ipod|blackberry|iemobile|opera/i) ? ua.match(/android|webos|iphone|ipad|ipod|blackberry|iemobile|opera/i)[0] : false;

    switch (device) {
        case 'iphone':
        case 'ipad':
        case 'ipod':
            function iOSversion() {
                if (/iP(hone|od|ad)/.test(plat)) {
                    // supports iOS 2.0 and later: <http://bit. ly/TJjs1V>
                    var v = (navigator.appVersion).match(/OS (\d+)_(\d+)_?(\d+)?/);
                    return [parseInt(v[1], 10), parseInt(v[2], 10), parseInt(v[3] || 0, 10)];
                }
            }

            var ver = iOSversion() || [0];

            if (ver[0] >= 6) {
                protocol = `http://maps.apple.com/?daddr=${game.Location.Latitude},${game.Location.Longitude},-122.031375`;
            }
            else {
                protocol = `http://maps.google.com/maps?q=${game.Location.Latitude},${game.Location.Longitude}&z=15`;
            }
            break;

        default:
            protocol = `http://maps.google.com/maps?q=${game.Location.Latitude},${game.Location.Longitude}&z=15`;
            break;
    }

    return protocol;
}

function RemoveLocation() {
    return new Promise((res, rej) => {
        var locElem = $('.schedule__singleday__location');
        locElem.removeClass('popin');

        locElem.animate({ size: 0 }, {
            step: (now) => {
                locElem.css({ 'transform': `scale(${now})` });
            }, complete: () => {
                locElem.remove();
                res("done");
            }
        });
    });
}