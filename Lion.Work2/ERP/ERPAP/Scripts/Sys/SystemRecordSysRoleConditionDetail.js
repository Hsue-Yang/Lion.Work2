var _formElement;

function SystemRecordSysRoleConditionDetailForm_onLoad(formElement) {
    _formElement = formElement;
    $('#QueryBuilderBox').queryBuilder({ modelName: 'SystemRoleConditionGroupRule', isReadOnly: true, filters: filters, rules: rules });
    $('input,select').each(
        function(idx, e) {
            $(e).attr('disabled', true);
        }
    );
}
