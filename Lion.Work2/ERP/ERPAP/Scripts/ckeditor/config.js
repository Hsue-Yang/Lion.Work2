/**
 * @license Copyright (c) 2003-2013, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.html or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function (config) {
	// Define changes to default configuration here.
	// For the complete reference:
	// http://docs.ckeditor.com/#!/api/CKEDITOR.config

	// The toolbar groups arrangement, optimized for two toolbar rows.
	config.toolbar = 'Lion';
	config.templates_files = ['/Content/ckeditor/plugins/templates/templates/default.js'];
	config.enterMode = CKEDITOR.ENTER_BR;
	config.shiftEnterMode = CKEDITOR.ENTER_P;
	config.format_tags = "p";
	config.toolbar_Lion =
	[
		 ['Preview'], ['Bold'], ['TextColor'], ['Link', 'Unlink']
	];
	config.colorButton_colors = '000,fff,f00,0f0,00f';
	config.colorButton_enableMore = false;
	config.htmlEncodeOutput = true;
};
