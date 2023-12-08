;
(function ($) {
    $.widget('ui.formBuilder', {
        controls: {
            Key :'ControlKey',
            TextBox: 'TextBox',
            Date: 'Date',
            DropdownList: 'DropdownList',
            Radio: 'Radio',
            CheckBox: 'CheckBox'
        },
        options: {
            modelName: '',
            isReadOnly: false,
            filters: [],
            rules: {}
        },
        _create: function () {
            var self = this;
            this.id = this.element.attr('id');
            this._tblsquare2 = $(document.createElement('table')).addClass('tblsquare2');
            var ul = this._generateControls();
            this._filedList = $(document.createElement('td')).css('width', '30%').append(ul);
            this._targetBlock = $(document.createElement('td')).css('width', '70%');
            var tr = $(document.createElement('tr')).append(this._filedList).append(this._targetBlock);
            this._tblsquare2.append($(document.createElement('tbody')).append(tr));
            this.element.append(this._tblsquare2);

            var addTableButtion =
                $(document.createElement('input'))
                    .attr('type', 'button')
                    .addClass('btn')
                    .val('新增表格')
                    .click(function() {
                        self._generateFrom({
                            confirm_onClick: function() {
                                addTableButtion.remove();
                            }
                        });
                    });

            this._targetBlock.append(addTableButtion);
        },
        _initsortable: function () {
            var self = this;
            $('.connectedSortable').sortable({
                connectWith: '.connectedSortable',
                cursor: 'all-scroll',
                forcePlaceholderSize: true,
                placeholder: 'roleItemPlaceholder',
                start: function (e, ui) {
                    ui.placeholder.width(ui.item.width());
                    ui.placeholder.height(ui.item.height());
                    ui.placeholder.addClass(ui.item.attr('class'));
                    ui.placeholder.css({
                        'border': '2px solid #5599FF',
                        'border-style': 'dashed'
                    });
                },
                opacity: 0.5,
                distance: 0.1,
                receive: function (event, ui) {
                    var targetBox = $(event.target).closest('td');
                    self._generateControl(event, ui, targetBox);
                    sortable.sortable("cancel");
                    return false;
                }
            });
        },
        _generateControls: function () {
            var ul = $(document.createElement('ul')).addClass('connectedSortable');
            var textbox = $(document.createElement('li')).addClass('tagstyle').html('文字欄位');
            var date = $(document.createElement('li')).addClass('tagstyle').html('日期');
            var dropdownList = $(document.createElement('li')).addClass('tagstyle').html('選單');
            var radio = $(document.createElement('li')).addClass('tagstyle').html('單選群組');
            var checkbox = $(document.createElement('li')).addClass('tagstyle').html('核取方塊');

            $.data(textbox[0], this.controls.Key, this.controls.TextBox);
            $.data(date[0], this.controls.Key, this.controls.Date);
            $.data(dropdownList[0], this.controls.Key, this.controls.DropdownList);
            $.data(radio[0], this.controls.Key, this.controls.Radio);
            $.data(checkbox[0], this.controls.Key, this.controls.CheckBox);

            ul.append(textbox);
            ul.append(date);
            ul.append(dropdownList);
            ul.append(radio);
            ul.append(checkbox);
            return ul;
        },
        _generateFrom: function (para) {
            var self = this;
            var row = $('<input type=text size=4 />');
            var col = $('<input type=text size=4 />');
            var table = $(document.createElement('table')).addClass('tblvertical');

            table.append($(document.createElement('tr'))
                .append($(document.createElement('th')).html('列'))
                .append($(document.createElement('td')).html(row))
                .append($(document.createElement('th')).html('欄'))
                .append($(document.createElement('td')).html(col)));

            this._generateDialog({
                conent: table,
                confirm_onClick: function () {

                    var tb = $(document.createElement('table')).addClass('tblsquare');
                    var tbody = $(document.createElement('tbody'));
                    
                    for (var i = 0; i < parseInt(row.val()); i++) {
                        var tr = $(document.createElement('tr'));
                        for (var j = 0; j < parseInt(col.val()); j++) {
                            var label = $(document.createElement('lable')).attr('contenteditable', true).html('編輯文字');
                            var th = $(document.createElement('th')).append(label);
                            var td = $(document.createElement('td')).addClass('connectedSortable').html('&nbsp;');
                            tr.append(th).append(td);
                        }
                        tbody.append(tr);
                    }
                    tb.append(tbody);

                    self._targetBlock.append(tb);
                    self._initsortable();

                    $(this).dialog('close');

                    if (typeof (para.confirm_onClick) === 'function') {
                        para.confirm_onClick();
                    }
                }
            });

        },
        _generateControl: function(event, ui, targetBox) {
            var controlType = $.data(event.target, this.controls.Key);
            switch (controlType) {
            case this.controls.TextBox:
                this._generateTextBoxControl(event, ui, targetBox);
                break;
            case this.controls.Date:
                this._generateDateControl(event, ui, targetBox);
                break;
            case this.controls.DropdownList:
                this._generateDropdownListControl(event, ui, targetBox);
                break;
            case this.controls.Radio:
                this._generateRadioControl(event, ui, targetBox);
                break;
            case this.controls.CheckBox:
                this._generateCheckBoxControl(event, ui, targetBox);
                break;
            default:
            }
        },
        _generateTextBoxControl: function (event, ui, targetBox) {
            var textbox = $('<input type=text />');
            var maxlength = $('<input type=text size=4 />');
            var isRequired = $('<input type=checkbox />');
            var table = $(document.createElement('table')).addClass('tblvertical');
            var tr = $(document.createElement('tr'))
                .append($(document.createElement('th')).html('欄位名稱'))
                .append($(document.createElement('td')).html(textbox))
                .append($(document.createElement('th')).html('長度'))
                .append($(document.createElement('td')).html(maxlength))
                .append($(document.createElement('th')).html('必填'))
                .append($(document.createElement('td')).html(isRequired));

            table.append(tr);

            this._generateDialog({
                conent: table,
                confirm_onClick: function() {
                    var input = $(document.createElement('input')).attr({
                        name: textbox.val()
                    });

                    if ($(isRequired).is(':checked')) {
                        input.attr('required', 'required');
                    }

                    if (maxlength.val() !== '') {
                        input.attr('maxlength', maxlength.val());
                    }

                    targetBox.append(input);
                    $(this).dialog('close');
                }
            });
        },
        _generateDateControl: function (event, ui, targetBox) {
            var self = this;
            var textbox = $('<input type=text />');
            var isRequired = $('<input type=checkbox />');
            var table = $(document.createElement('table')).addClass('tblvertical');
            var tr = $(document.createElement('tr'))
                .append($(document.createElement('th')).html('欄位名稱'))
                .append($(document.createElement('td')).html(textbox))
                .append($(document.createElement('th')).html('必填'))
                .append($(document.createElement('td')).html(isRequired));

            table.append(tr);

            this._generateDialog({
                conent: table,
                confirm_onClick: function () {
                    var input = $(document.createElement('input')).attr({
                        name: textbox.val(),
                        inputtype: 'TextBoxDatePicker'
                    });

                    if ($(isRequired).is(':checked')) {
                        input.attr('required', 'required');
                    }

                    targetBox.append(input);
                    _Datepicker(self._tblsquare2);
                    $(this).dialog('close');
                }
            });
        },
        _generateDropdownListControl: function (event, ui, targetBox) {
            var self = this;
            var textbox = $('<input type=text />');
            var isRequired = $('<input type=checkbox />');
            var table = $(document.createElement('table')).addClass('tblvertical');
            var tr = $(document.createElement('tr'))
                .append($(document.createElement('th')).html('欄位名稱'))
                .append($(document.createElement('td')).html(textbox))
                .append($(document.createElement('th')).html('必填'))
                .append($(document.createElement('td')).html(isRequired));

            table.append(tr);

            var tblist = $(document.createElement('table')).addClass('tblzebra');
            tr = $(document.createElement('tr'))
                .append($(document.createElement('th')).html('文字'))
                .append($(document.createElement('th')).html('值'))
                .append($(document.createElement('th')).html('刪除'));
            tblist.append(tr);

            tr = $(document.createElement('tr'))
                .append($(document.createElement('td')).attr('colspan', 4).append(tblist));
            table.append(tr);

            var addRow_onClick = function () {
                var tr = $(document.createElement('tr'));
                var td = $(document.createElement('td'));
                var text = $('<input type=text name=optionText />');
                var value = $('<input type=text name=optionValue />');

                var alink = self._generateRemoveButton();
                alink.click(function() { tr.remove(); });

                tr.append(td.clone().append(text));
                tr.append(td.clone().append(value));
                tr.append(td.clone().append(alink));

                tblist.append(tr);
            };

            addRow_onClick();
            addRow_onClick();

            var buttions = {
                click: {
                    'class': 'btn',
                    text: 'Confirm',
                    id: 'DialogConfirm',
                    click: function () {
                        var select = $(document.createElement('select'));

                        $('tr:eq(0)', tblist).nextAll().each(function(index, tr) {
                            var text = $('input[name=optionText]', tr).val();
                            var value = $('input[name=optionValue]', tr).val();
                            var option = $(document.createElement('option')).attr('value', value).html(text);
                            select.append(option);
                        });
                        
                        if ($(isRequired).is(':checked')) {
                            select.attr('required', 'required');
                        }

                        targetBox.append(select);
                        $(this).dialog('close');
                    }
                },
                addRow: {
                    'class': 'btn',
                    text: 'AddRow',
                    id: 'AddRow',
                    click: function () {
                        addRow_onClick();
                    }
                }
            };

            this._generateDialog({
                conent: table,
                buttions: buttions
            });
        },
        _generateRadioControl: function (event, ui, targetBox) {
            var self = this;
            var textbox = $('<input type=text />');
            var isRequired = $('<input type=checkbox />');
            var table = $(document.createElement('table')).addClass('tblvertical');
            var tr = $(document.createElement('tr'))
                .append($(document.createElement('th')).html('欄位名稱'))
                .append($(document.createElement('td')).html(textbox))
                .append($(document.createElement('th')).html('必填'))
                .append($(document.createElement('td')).html(isRequired));

            table.append(tr);

            var tblist = $(document.createElement('table')).addClass('tblzebra');
            tr = $(document.createElement('tr'))
                .append($(document.createElement('th')).html('文字'))
                .append($(document.createElement('th')).html('值'))
                .append($(document.createElement('th')).html('刪除'));
            tblist.append(tr);

            tr = $(document.createElement('tr'))
                .append($(document.createElement('td')).attr('colspan', 4).append(tblist));
            table.append(tr);

            var addRow_onClick = function () {
                var tr = $(document.createElement('tr'));
                var td = $(document.createElement('td'));
                var text = $('<input type=text name=radioText />');
                var value = $('<input type=text name=radioValue />');

                var alink = self._generateRemoveButton();
                alink.click(function () { tr.remove(); });

                tr.append(td.clone().append(text));
                tr.append(td.clone().append(value));
                tr.append(td.clone().append(alink));

                tblist.append(tr);
            };

            addRow_onClick();
            addRow_onClick();

            var buttions = {
                click: {
                    'class': 'btn',
                    text: 'Confirm',
                    id: 'DialogConfirm',
                    click: function () {

                        $('tr:eq(0)', tblist).nextAll().each(function (index, tr) {
                            var name = textbox.val();
                            var id = name + index;
                            var value = $('input[name=radioValue]', tr).val();
                            var radio = $(document.createElement('input'))
                                .attr({
                                    id: id,
                                    name: name,
                                    'type': 'radio'
                                }).val(value);

                            var text = $(document.createElement('label')).attr('for', id).html($('input[name=radioText]', tr).val());

                            targetBox.append(radio);
                            targetBox.append(text);
                        });
                        
                        $(this).dialog('close');
                    }
                },
                addRow: {
                    'class': 'btn',
                    text: 'AddRow',
                    id: 'AddRow',
                    click: function () {
                        addRow_onClick();
                    }
                }
            };

            this._generateDialog({
                conent: table,
                buttions: buttions
            });
        },
        _generateCheckBoxControl: function (event, ui, targetBox) {
            var self = this;
            var textbox = $('<input type=text />');
            var isRequired = $('<input type=checkbox />');
            var table = $(document.createElement('table')).addClass('tblvertical');
            var tr = $(document.createElement('tr'))
                .append($(document.createElement('th')).html('欄位名稱'))
                .append($(document.createElement('td')).html(textbox))
                .append($(document.createElement('th')).html('必填'))
                .append($(document.createElement('td')).html(isRequired));

            table.append(tr);

            var tblist = $(document.createElement('table')).addClass('tblzebra');
            tr = $(document.createElement('tr'))
                .append($(document.createElement('th')).html('文字'))
                .append($(document.createElement('th')).html('值'))
                .append($(document.createElement('th')).html('刪除'));
            tblist.append(tr);

            tr = $(document.createElement('tr'))
                .append($(document.createElement('td')).attr('colspan', 4).append(tblist));
            table.append(tr);

            var addRow_onClick = function () {
                var tr = $(document.createElement('tr'));
                var td = $(document.createElement('td'));
                var text = $('<input type=text name=checkboxText />');
                var value = $('<input type=text name=checkboxValue />');

                var alink = self._generateRemoveButton();
                alink.click(function () { tr.remove(); });

                tr.append(td.clone().append(text));
                tr.append(td.clone().append(value));
                tr.append(td.clone().append(alink));

                tblist.append(tr);
            };

            addRow_onClick();
            addRow_onClick();

            var buttions = {
                click: {
                    'class': 'btn',
                    text: 'Confirm',
                    id: 'DialogConfirm',
                    click: function () {

                        $('tr:eq(0)', tblist).nextAll().each(function (index, tr) {
                            var name = textbox.val();
                            var id = name + index;
                            var value = $('input[name=checkboxValue]', tr).val();
                            var radio = $(document.createElement('input'))
                                .attr({
                                    id: id,
                                    name: name,
                                    'type': 'checkbox'
                                }).val(value);

                            var text = $(document.createElement('label')).attr('for', id).html($('input[name=checkboxText]', tr).val());

                            targetBox.append(radio);
                            targetBox.append(text);
                        });

                        $(this).dialog('close');
                    }
                },
                addRow: {
                    'class': 'btn',
                    text: 'AddRow',
                    id: 'AddRow',
                    click: function () {
                        addRow_onClick();
                    }
                }
            };

            this._generateDialog({
                conent: table,
                buttions: buttions
            });
        },
        _generateDialog: function (para) {
            var dialog = $(document.createElement('div'));
            var buttions = {
                click: {
                    'class': 'btn',
                    text: 'Confirm',
                    id: 'DialogConfirm',
                    click: para.confirm_onClick
                }
            };

            if (para.buttions) {
                buttions = para.buttions;
            }

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
                buttons: buttions
            });
        },
        _generateRemoveButton: function() {
            return $(document.createElement('a'))
                .attr({
                    'class': 'ico-href',
                    'href': 'javascript:void(0);'
                })
                .html($(document.createElement('span'))
                    .addClass('icon-stack')
                    .append($(document.createElement('i')).addClass('icon-sign-blank').addClass('icon-stack-base'))
                    .append($(document.createElement('i')).addClass('icon-light').addClass('icon-remove')));
        }
    });

    //$.extend($.ui.queryBuilder, {
    //    locale: {
    //        DeleteButton: '刪除',
    //        DeleteGroupRoleButton: '刪除群組',
    //        AddGroupRoleButton: '新增群組',
    //        AddRoleRuleButton: '新增規則',
    //        ANDCondition: '且(AND)',
    //        ORCondition: '或(OR)'
    //    },
    //    operators: [
    //        { Value: 'Equal', Text: '=' },
    //        { Value: 'NotEqual', Text: '<>' },
    //        //{ Value: 'In', Text: 'In' },
    //        //{ Value: 'NotIn', Text: 'Not In' },
    //        { Value: 'Less', Text: '<' },
    //        { Value: 'LessOrEqual', Text: '<=' },
    //        { Value: 'Greater', Text: '>' },
    //        { Value: 'GreaterOrEqual', Text: '>=' },
    //        //{ Value: 'Like', Text: 'Like' },
    //        { Value: 'IsNull', Text: 'Is Null' },
    //        { Value: 'IsNotNull', Text: 'Is Not Null' }
    //    ]
    //});

})(jQuery);