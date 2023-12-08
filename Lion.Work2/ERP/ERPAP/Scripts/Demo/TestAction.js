var _formElement;

function DemoForm_onLoad(formElement) {
    _formElement = formElement;

    //var roleTagObject = $('td[name=source], td[name=target]', _formElement);
    $('span.tagstyle', _formElement).css('cursor', 'pointer');

    var style = document.createElement('style');
    style.type = 'text/css';
    style.innerHTML = '.roleItemPlaceholder {border: 2px solid #AAA;border-style: dashed;}';
    document.getElementsByTagName('head')[0].appendChild(style);
    
    $('#divContent2').formBuilder();

    //roleTagObject.sortable({
    //    connectWith: roleTagObject,
    //    cursor: 'all-scroll',
    //    forcePlaceholderSize: true,
    //    placeholder: 'roleItemPlaceholder',
    //    start: function (e, ui) {
    //        ui.placeholder.width(ui.item.width());
    //        ui.placeholder.height(ui.item.height());
    //        ui.placeholder.addClass(ui.item.attr('class'));
    //        ui.placeholder.css({
    //            'border': '2px solid #5599FF',
    //            'border-style': 'dashed'
    //        });
    //    },
    //    opacity: 0.5,
    //    distance: 0.1,
    //    receive: function (event, ui) {
    //        var targetBox = $(event.target).closest('td');
    //        GenerateComboBoxHelper(event, ui, targetBox);
    //        roleTagObject.sortable("cancel");
    //        return false;
    //    }
    //});

    return true;
}

function GenerateTextBoxHelper(event, ui, targetBox) {
    var inputName = $('<input type=text />');
    var isRequired = $('<input type=checkbox />');
    var table = $(document.createElement('table')).addClass('tblvertical');
    var tr = $(document.createElement('tr'))
        .append($(document.createElement('th')).html('欄位名稱'))
        .append($(document.createElement('td')).html(inputName))
        .append($(document.createElement('th')).html('必填'))
        .append($(document.createElement('td')).html(isRequired));

    table.append(tr);

    GenerateDialog({
        conent: table,
        confirm_onClick: function () {
            var input = $(document.createElement('input')).attr({
                name: inputName.val()
            });

            if ($(isRequired).is(':checked')) {
                input.attr('required', 'required');
            }

            targetBox.append(input);
            $(this).dialog('close');
        }
    });
}

function GenerateComboBoxHelper(event, ui, targetBox) {
    var inputName = $('<input type=text />');
    var isRequired = $('<input type=checkbox />');
    var table = $(document.createElement('table')).addClass('tblvertical');
    var tr = $(document.createElement('tr'))
        .append($(document.createElement('th')).html('欄位名稱'))
        .append($(document.createElement('td')).html(inputName))
        .append($(document.createElement('th')).html('必填'))
        .append($(document.createElement('td')).html(isRequired));

    table.append(tr);

    tr = $(document.createElement('tr'))
        .append($(document.createElement('th')).html('Option'))
        .append(
            $(document.createElement('td'))
            .attr({ colspan: '3' })
            .append($('<input type=text />'))
            .append($('<input type=text />'))
            .append($('<input type=checkbox />'))
        );

    table.append(tr);

    GenerateDialog({
        conent: table,
        confirm_onClick: function () {
            var input = $(document.createElement('input')).attr({
                name: inputName.val()
            });

            if ($(isRequired).is(':checked')) {
                input.attr('required', 'required');
            }

            targetBox.append(input);
            $(this).dialog('close');
        }
    });
}

function GenerateDialog(para) {
    var dialog = $(document.createElement('div'));

    dialog.append(para.conent);
    
    dialog.dialog({
        title: '',
        autoOpen: true,
        modal: true,
        resizable: false,
        height: 400,
        width: 600,
        open: function (event, ui) {
          
        },
        close: function () {
            dialog.dialog('destroy').remove();
        },
        buttons: {
            click: {
                'class': 'btn',
                text: 'Confirm',
                id: 'DialogConfirm',
                click: para.confirm_onClick
            }
        }
    });
}
