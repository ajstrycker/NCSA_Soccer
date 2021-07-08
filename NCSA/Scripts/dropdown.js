$(document).ready(function () {

    $('select').each(function () {
        var $this = $(this), numberOfOptions = $(this).children('option').length;

        $this.addClass('select-hidden');
        var id = $this.attr('id');
        if (id === 'gender' || id === 'shirtSize') {
            $this.wrap('<div class="select form__group group-1-3"></div>');
        }
        else if (id === 'teamId') {
            $this.wrap('<div class="select form__group group-3-5"></div>');
        }
        var labelName = $("#" + id + " option")[0].text.replace(/-/g, '');
        $this.after(`<label class="form__label-select" for="${$this.attr('id')}">${labelName}</label>`);
        $this.after('<div class="select-styled"></div>');

        var $styledSelect = $this.next('div.select-styled');
        $styledSelect.text($this.children('option').eq(0).text());

        var $list = $('<ul />', {
            'class': 'select-options'
        }).insertAfter($styledSelect);

        for (var i = 0; i < numberOfOptions; i++) {
            $('<li />', {
                text: $this.children('option').eq(i).text(),
                rel: $this.children('option').eq(i).val()
            }).appendTo($list);
        }

        var $listItems = $list.children('li');

        $styledSelect.click(function (e) {
            e.stopPropagation();
            $('div.select-styled.active').not(this).each(function () {
                $(this).removeClass('active').next('ul.select-options').hide();
            });
            $(this).toggleClass('active').next('ul.select-options').toggle();
        });

        $listItems.click(function (e) {
            e.stopPropagation();
            $styledSelect.text($(this).text()).removeClass('active');
            $this.val($(this).attr('rel'));
            $list.hide();
            //console.log($this.val());
        });

        $(document).click(function () {
            $styledSelect.removeClass('active');
            $list.hide();
        });
    });
});
