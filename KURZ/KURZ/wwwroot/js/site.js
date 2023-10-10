
(function ($) {
	'use strict';

	$(document).ready(function () {
		

		$('#CreateUser').on('submit', function () {
			var email = $('#EMAIL').val();
			$('#USERNAME').val(email);
		});

		$('#EditUser').on('submit', function () {
			var email = $('#EMAIL').val();
			$('#USERNAME').val(email);
		});
		$('#RegisterStudents').on('submit', function () {
			var email = $('#EMAIL').val();
			$('#USERNAME').val(email);
		});
		
		$('#RegisterTeachers').on('submit', function () {
			var email = $('#EMAIL').val();
			$('#USERNAME').val(email);
		});

	});

})(jQuery);
