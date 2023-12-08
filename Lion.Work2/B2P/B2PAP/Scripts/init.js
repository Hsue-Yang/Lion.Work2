$(function () {

		//datepicker
		$('.dp').datepicker({
			language: "zh-TW",
			autoclose: true
		});
		
		
		
		var selection1 = $('#function-bar .dropdown-menu');
		var selection2 = $('#topnav > ul');
		var selection2_sub = $('#topnav .hasmenu .navtxt');
		var selection3 = $('#nav > ul');
		var selection3_sub = $('#nav .hasmenu .navtxt');
		
		//M版右上角選單
		$('#navicon1').click(
			function(){
				selection2.hide();
				selection3.hide();
				$('#navicon2').removeClass('active');
				$('#navicon3').removeClass('active');
				if ( selection1.is(':visible') ) {
					selection1.hide();
					$(this).removeClass('active');
				} else {
					selection1.show();
					$(this).addClass('active');
				}
			}
		);
		
		$('#navicon2').click(
			function(){
				selection1.hide();
				selection3.hide();
				$('#navicon1').removeClass('active');
				$('#navicon3').removeClass('active');
				if ( selection2.is(':visible') ) {
					selection2.hide();
					$(this).removeClass('active');
				} else {
					selection2.show();
					$(this).addClass('active');
				}
			}
		);
		selection2_sub.click(
			function(){
				var sub_menu = $(this).siblings($('.subnav'));
				if ( sub_menu.is(':visible') ) {
					sub_menu.hide();
					$(this).removeClass('active');
				} else {
					sub_menu.show();
					$(this).addClass('active');
				}
			}
		);
		
		$('#navicon3').click(
			function(){
				selection1.hide();
				selection2.hide();
				$('#navicon1').removeClass('active');
				$('#navicon2').removeClass('active');
				if ( selection3.is(':visible') ) {
					selection3.hide();
					$(this).removeClass('active');
				} else {
					selection3.show();
					$(this).addClass('active');
				}
			}
		);
		selection3_sub.click(
			function(){
				var sub_menu = $(this).siblings($('.subnav'));
				if ( sub_menu.is(':visible') ) {
					sub_menu.hide();
					$(this).removeClass('active');
				} else {
					sub_menu.show();
					$(this).addClass('active');
				}
			}
		);
		
		$('#navicon4').click(
			function(){
			    $('head').find('link[media="screen and (min-width:320px) and (max-width:1023px)"]').remove();
                $('head').find('meta[name="viewport"]').remove();
				$(window).unbind('resize',winResize);
				selection1.css('display','');
				selection2.css('display','block');
				selection2_sub.siblings($('.subnav')).css('display','');
				selection3.css('display','block');
				selection3_sub.siblings($('.subnav')).css('display','');
			}
		);
		
		
		
		//window resize : reset
		$(window).bind('resize',winResize);
		
		function winResize() {
			var win = $(this);
			if(win.width()>=640){
				selection1.css('display','');
				selection2.css('display','block');
				selection2_sub.siblings($('.subnav')).css('display','');
				selection3.css('display','block');
				selection3_sub.siblings($('.subnav')).css('display','');
			}
			if(win.width()>=320&&win.width()<900){
				selection1.css('display','none');
				selection2.css('display','none');
				selection3.css('display','none');
				$('#navicon1').removeClass('active');
				$('#navicon2').removeClass('active');
				selection2_sub.removeClass('active');
				$('#navicon3').removeClass('active');
				selection3_sub.removeClass('active');
			}
		}
		
	}
);

//color change
function changeColor($color) {
	switch($color) {
		case 'green':
			$('body').removeClass().addClass('greenStyle');
			break;
		case 'red':
			$('body').removeClass().addClass('redStyle');
			break;
	}
}	