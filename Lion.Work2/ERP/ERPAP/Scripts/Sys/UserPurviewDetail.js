var _formElement;

function UserPurviewDetailForm_onLoad(formElement) {
    _formElement = formElement;
    $('div.advancedFilter span.tagstyle').each(function (idx, el) {
        var code = $('input[name$=CodeID]', el).val();
        var purviewCodeType = $('input[name$=PurviewCodeType]', el).val();
        var purviewOp = $('input[name$=PurviewOP]:checked', el).val();
        $.data(el, code + '|' + purviewCodeType, purviewOp);
    });

    var filterIcon = $('div.advancedFilter .ico-href');

    filterIcon.click(function() {
        var self = $(this);
        self.find('i').toggleClass('icon-angle-down');
        self.parents('div.advancedFilter').find('div.filterCont').toggleClass('clickShow');
    });

    $('.filterCont').each(function(idx, el) {
        if ($('span.tagstyle', el).length > 0) {
            $('input.txtbtn', el).css('color', 'rgb(162, 162, 162)');
        }
    });

    $('input.txtbtn').click(function() {
        var self = $(this);
        self.parents('div.filterCont').find('.tagstyle').remove();
        self.css({ 'color': '#ffffff' });
    });

    $('select[name=Com],select[name=Unit],select[name=Country]').combobox({
        isRemoveButton: false,
        onChange: function (event) {
            $('input.txtbtn', event.select.closest('.filterCont')).css('color', 'rgb(162, 162, 162)');

            if ($('input[name$=CodeID][value= "' + event.select.val() + '"]', event.input.closest('li')).length === 0) {
                var divNm, tagID;
                var tagText = $(event.input).val();
                var codeType = $(event.select)[0].id;
                
                switch (codeType) {
                    case 'Unit':
                        tagID = 'UnitTag';
                        divNm = 'UnitTagListBox';
                        break;
                    case 'Com':
                        tagID = 'ComTag';
                        divNm = 'ComTagListBox';
                        break;
                    case 'Country':
                        tagID = 'CountryTag';
                        divNm = 'CountryTagListBox';
                        break;
                }

                var para = {
                    id: tagID,
                    text: tagText,
                    isRevmove: true
                };
                var purviewQuery =
                    $(document.createElement('input'))
                        .attr({
                            'type': 'radio',
                            'checked': 'checked',
                            'id': 'UserPurviewInfoList[9999].PurList[9999].PurviewOP',
                            'name': 'UserPurviewInfoList[9999].PurList[9999].PurviewOP',
                            'value': 'Q',
                            onclick: '_InputRadioButton_onClick(this);'
                        }).after(JsMsg_Query);

                var purviewUpdate =
                    $(document.createElement('input'))
                        .attr({
                            'type': 'radio',
                            'id': 'UserPurviewInfoList[9999].PurList[9999].PurviewOP',
                            'name': 'UserPurviewInfoList[9999].PurList[9999].PurviewOP',
                            'value': 'U',
                            onclick: '_InputRadioButton_onClick(this);'
                        }).after(JsMsg_Update);

                var opRadioBtn = $(document.createElement('span')).append(purviewQuery).append(purviewUpdate);

                var purviewCodeType =
                    $(document.createElement('input'))
                        .attr({
                            'type': 'hidden',
                            'id': 'UserPurviewInfoList[9999].PurList[9999].PurviewCodeType',
                            'name': 'UserPurviewInfoList[9999].PurList[9999].PurviewCodeType',
                            value: codeType
                        });

                var codeId =
                    $(document.createElement('input'))
                        .attr({
                            'type': 'hidden',
                            'id': 'UserPurviewInfoList[9999].PurList[9999].CodeID',
                            'name': 'UserPurviewInfoList[9999].PurList[9999].CodeID',
                            value: $(event.select).val()
                        });
                para.elementList = [purviewCodeType, codeId];
                var tagStyle = Html.Tagstyle(para);
                $('div[id = "' + divNm + '"]', event.input.closest('li')).append(tagStyle.append('<br>').append(opRadioBtn));

                $.data(tagStyle[0], $(event.select).val() + '|' + codeType, 'Q');
                _PurviewTagReName();
            }
            event.select.val('');
            event.input.val('');
            return false;
        }
    });

    $('.ui-combobox-toggle .ui-icon').css('margin-top', '-10px');
}

function UnitTagRemove_onClick(srcElement, keys) {
    _TagStyleRemove(srcElement);
}

function ComTagRemove_onClick(srcElement, keys) {
    _TagStyleRemove(srcElement);
}

function CountryTagRemove_onClick(srcElement, keys) {
    _TagStyleRemove(srcElement);
}

function AddButton_onClick(srcElement) {
    var result = _FormValidation();
    if (result) {
        $.blockUI({ message: '' });
        $('#ExecAction').val(_ActionTypeUpdate);
        return true;
    }
    return false;
}

function CloseButton_onClick(srcElement) {
    _windowClose();
}

function PurviewOP_onClick(srcElement) {
    var tagStyle = $(srcElement).closest('span.tagstyle');
    var code = $('input[name$=CodeID]', tagStyle).val();
    $.data(tagStyle[0], code + '|' + $('input[name$=PurviewCodeType]', tagStyle).val(), srcElement.value);
}

function _TagStyleRemove(srcElement) {
    var tagStyle = $(srcElement).closest('span.tagstyle');
    var code = $('input[name$=CodeID]', tagStyle).val();
    var purviewCodeType = $('input[name$=PurviewCodeType]', tagStyle).val();

    $.removeData(tagStyle[0], code + '|' + purviewCodeType);
    tagStyle.remove();
    _PurviewTagReName();
}

function _PurviewTagReName() {
    var rowIndex = $('div.advancedFilter').length - 1;

    $('div.advancedFilter').each(function (idx, el) {
        $('input[name$=PurviewID]', el).attr({
            id: 'UserPurviewInfoList[' + rowIndex + '].PurviewID',
            name: 'UserPurviewInfoList[' + rowIndex + '].PurviewID'
        });

        var rowElIndex = $('span.tagstyle', el).length - 1;
        $('span.tagstyle', el).each(function (idxTag, elTag) {

            $('input[name$=PurviewOP]', elTag).attr({
                id: 'UserPurviewInfoList[' + rowIndex + '].PurList[' + rowElIndex + '].PurviewOP',
                name: 'UserPurviewInfoList[' + rowIndex + '].PurList[' + rowElIndex + '].PurviewOP'
            });
            $('input[name$=PurviewCodeType]', elTag).attr({
                id: 'UserPurviewInfoList[' + rowIndex + '].PurList[' + rowElIndex + '].PurviewCodeType',
                name: 'UserPurviewInfoList[' + rowIndex + '].PurList[' + rowElIndex + '].PurviewCodeType'
            });
            $('input[name$=CodeID]', elTag).attr({
                id: 'UserPurviewInfoList[' + rowIndex + '].PurList[' + rowElIndex + '].CodeID',
                name: 'UserPurviewInfoList[' + rowIndex + '].PurList[' + rowElIndex + '].CodeID'
            });

            rowElIndex--;
        });
        rowIndex--;
    });

    $('div.advancedFilter span.tagstyle').each(function (idx, el) {
        var code = $('input[name$=CodeID]', el).val();
        var purviewCodeType = $('input[name$=PurviewCodeType]', el).val();
        var purviewOp = $.data(el, code + '|' + purviewCodeType);

        $('input[name$=PurviewOP][value=' + purviewOp + ']', el).attr('checked', 'checked');
    });
}
