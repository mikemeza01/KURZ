(function ($) {
    $(function () {

        var languageDatatable = {
            url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json',
        };

        $(document).ready(function () {

            var tableUsers = $('#TableUsers');
            var tableAdvices = $('#TableAdvices');
            var tableCategories = $('#TableCategories');
            var tableTeacherGrades = $('#TableTeacherGrades');
            var tableSubcategories = $('#TableSubcategories');
            var tableList = $('#TableList');
            var tableTopicsByTeacher = $('#TableTopicsByTeacher');
            var TableAdvicesStudent = $('#TableAdvicesStudent');
            var tableTeacherRates = $('#TableTeacherRates');

            if (tableUsers.length > 0) {
                new DataTable('#TableUsers', {
                    language: languageDatatable
                });
            }
            if (tableAdvices.length > 0) {
                new DataTable('#TableAdvices', {
                    language: languageDatatable
                });
            }
            if (tableCategories.length > 0) {
                new DataTable('#TableCategories', {
                    language: languageDatatable,
                    "autoWidth": false,
                    "columns": [
                        { "width": "33.3%" },
                        { "width": "33,3%" },
                        { "width": "33,3%" },
                    ]
                });
            }

            if (tableSubcategories.length > 0) {
                new DataTable('#TableSubcategories', {
                    language: languageDatatable,
                    "autoWidth": false,
                    "columns": [
                        { "width": "25%" },
                        { "width": "25%" },
                        { "width": "25%" },
                        { "width": "25%" },
                    ]
                });
            }

            if (tableTeacherRates.length > 0) {
                new DataTable('#TableTeacherRates', {
                    language: languageDatatable,
          
                });
            }
            if (tableList.length > 0) {
                new DataTable('#TableList', {
                    language: languageDatatable
                });
            }

            if (tableTopicsByTeacher.length > 0) {
                new DataTable('#TableTopicsByTeacher', {
                    language: languageDatatable,
                    "columns": [
                        { "width": "20%" },
                        { "width": "20%" },
                        { "width": "20%" },
                        { "width": "20%" },
                        { "width": "20%" },
                        { "width": "25%" },
                    ]
                });
            }

            if (TableAdvicesStudent.length > 0) {
                new DataTable('#TableAdvicesStudent', {
                    language: languageDatatable,
                    "columns": [
                        { "width": "25%" },
                        { "width": "10%" },
                        { "width": "25%" },
                        { "width": "25%" },
                        { "width": "25%" },
                        { "width": "10%" },
                    ]
                });
            }

            if ($('#ID_SUBCATEGORY').val() === '1') {
                $('#ID_CATEGORY').prop('disabled', true);
            }

            
        });
    });
})(jQuery);