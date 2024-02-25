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

            if ($('#ID_SUBCATEGORY').val() === '1') {
                $('#ID_CATEGORY').prop('disabled', true);
            }

            
        });
    });
})(jQuery);