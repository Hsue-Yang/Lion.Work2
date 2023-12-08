var _formElement,_disPlayTypeDiv;

function SystemFunElmRoleForm_onLoad(formElement) {
    _formElement = formElement;
    _disPlayTypeDiv = ['DISPLAYRoleList', 'READ_ONLYRoleList', 'MASKINGRoleList', 'HIDERoleList'];

    SysRoleTagMoveable();
}

function SysRoleTagMoveable(srcElement) {
    var roleTagObject = $('td#SysRoleIDListBox, td[name$="RoleList"]', _formElement);
    $('span.tagstyle', _formElement).css('cursor', 'pointer');

    var style = document.createElement('style');
    style.type = 'text/css';
    style.innerHTML = '.roleItemPlaceholder {border: 2px solid #AAA;border-style: dashed;}';
    document.getElementsByTagName('head')[0].appendChild(style);

    roleTagObject.sortable({
        connectWith: roleTagObject,
        cursor: 'all-scroll',
        forcePlaceholderSize: true,
        placeholder: 'roleItemPlaceholder',
        start: function(e, ui) {
            ui.placeholder.width(ui.item.width());
            ui.placeholder.height(ui.item.height());
            ui.placeholder.addClass(ui.item.attr("class"));
            ui.placeholder.css({
                'border': '2px solid #5599FF',
                'border-style': 'dashed'
            });
        },
        opacity: 0.5,
        distance: 0.1,
        receive: function (event, ui) {
            var disPlayDivIDName = $(ui.item).closest('td').attr('name');
            var name = 'FunElmRoleDictionary[' + _disPlayTypeDiv.indexOf(disPlayDivIDName) + '].value[999].RoleID';
            $('input', ui.item).attr('id', name);
            $('input', ui.item).attr('name', name);

            FunElmRoleDictionaryReName();
        }
    });
}

function FunElmRoleDictionaryReName(srcElement) {
    for (var i = 0; i < _disPlayTypeDiv.length; i++) {
        $('input[name^="FunElmRoleDictionary"][name$="RoleID"][type="hidden"]', $('td[name=' + _disPlayTypeDiv[i] + ']')).each(function (idx, el) {
            var elIdNm = $(el).attr('name');
            $(el).attr('id', elIdNm.replace(/value\[\d+]/g, 'value[' + idx + ']'));
            $(el).attr('name', elIdNm.replace(/value\[\d+]/g, 'value[' + idx + ']'));
        });
    }
}

function SaveButton_onClick(srcElement) {
    var result = _FormValidation();
    $('#ExecAction').val(_ActionTypeUpdate);
    if (result) {
        $.blockUI({ message: '' });

        for (var i = 0; i < _disPlayTypeDiv.length; i++) {
            var length = $('input[name^="FunElmRoleDictionary"][name$="RoleID"][type="hidden"]',
                $('td[name=' + _disPlayTypeDiv[i] + ']')).length;

            if (length === 0) {
                var roleHiddenField = $(document.createElement('input')).attr({
                    'type': 'hidden',
                    'id': 'FunElmRoleDictionary[' + _disPlayTypeDiv.indexOf(_disPlayTypeDiv[i]) + '].value[0].RoleID',
                    'name': 'FunElmRoleDictionary[' + _disPlayTypeDiv.indexOf(_disPlayTypeDiv[i]) + '].value[0].RoleID'
                });
                $('td[name=' + _disPlayTypeDiv[i] + ']').append(roleHiddenField);
            }
        }

        return true;
    }

    _ShowJsErrMessageBox();
    return false;
}

function CloseButton_onClick(srcElement) {
    window.close();
}