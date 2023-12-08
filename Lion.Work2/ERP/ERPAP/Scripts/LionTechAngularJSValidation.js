//非同步取方式取得Server端的資料
angular
    .module("shareAJAX", [])
    .factory('ajax', Factory_AnJS);

function Factory_AnJS($http) {
    return {
        callAjax: function (Controller, Action, condition) {
            return $http({
                method: 'POST',
                url: '/' + Controller + '/' + Action,
                data: { condition: condition }
            });
        }
    }
}
//宣告Directive自訂屬性
angular
  .module('Valid', [])
  .directive('ngMatchRequire', function () {
      return {
          restrict: 'A',
          require: 'ngModel',
          link: function (scope, element, attrs, ctrl) {
              element.on('change', valid_on);
              function valid_on() {
                  scope.$applyAsync(function () {
                      var value = element.val();

                      if (value != "") {
                          ctrl.$setValidity('MatchRequire', true);
                      } else {
                          ctrl.$setValidity('MatchRequire', false);
                      }
                  });
              }
          }
      }
  });
  angular
  .module('Valid')
  .directive('ngMatchMaxlength', function () {
      var count = 0;
      var after;
      return {
          restrict: 'A',
          require: 'ngModel',
          link: function (scope, element, attrs, ctrl) {
              element.on('keyup', valid_on);
              function valid_on(event) {
                  scope.$applyAsync(function () {
                      if (attrs.id != after) {
                          count = 0;
                      }
                      after = attrs.id;

                      if (event.keyCode === 8) {
                          ctrl.$setValidity('MatchMaxlength', true);
                          if (count !== 0) {
                              if (event.target.value.length === 0 || event.target.value.length === 1) {
                                  count = 0;
                              } else {
                                  count = event.target.value.length;
                              }
                          }
                          return true;
                      }

                      if ((event.keyCode >= 48 && event.keyCode <= 57) ||
                         (event.keyCode >= 65 && event.keyCode <= 90) ||
                         (event.keyCode >= 96 && event.keyCode <= 105)) {
                          count++;
                          if (count > element.attr('ng-match-maxlength')) {
                              if (event.target.value.length == 10) {
                                  ctrl.$setValidity('MatchMaxlength', false);
                                  count = event.target.value.length - 1;
                                  return false;
                              }
                          }
                      }
                  });
              }
          }
      };
  });
angular
  .module('Valid')
  .directive('ngMatchShift', function () {
      return {
          restrict: 'A',
          require: 'ngModel',
          link: function (scope, element, attrs, ctrl) {
              element.on('keypress', valid_on);
              function valid_on(event) {
                  scope.$applyAsync(function () {
                      var key = String.fromCharCode(event.keyCode);
                      if ((key.toUpperCase() === key && key.toLowerCase() !== key && !event.shiftKey) ||
                                 (key.toUpperCase() === key && key.toLowerCase() !== key && event.shiftKey)) {
                          ctrl.$setValidity('MatchShift', false);
                      } else {
                          ctrl.$setValidity('MatchShift', true);
                          return true;
                      }
                  });
              }
          }
      };
  });