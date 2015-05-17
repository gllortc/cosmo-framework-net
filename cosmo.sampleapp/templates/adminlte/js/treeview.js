$(function () {

   $('.tree li:has(ul)').addClass('parent_li').find(' > span').attr('title', 'Collapse this branch');

   /*$('.tree li.parent_li > span').on('click', function (e) {
      var children = $(this).parent('li.parent_li').find(' > ul > li');
      if (children.is(":visible")) {
         children.hide('fast');
         $(this).attr('title', 'Expand this branch').find(' > i').addClass('glyphicon-chevron-right').removeClass('glyphicon-chevron-down');
      } else {
         $(children).each(function (index, element) {
            // element.show();
         });
         // children.show('fast');
         $(this).attr('title', 'Collapse this branch').find(' > i').addClass('glyphicon-chevron-down').removeClass('glyphicon-chevron-right');
      }
      e.stopPropagation();
   });*/

   $('.tree li.parent_li > span').on('click', function (e) {
      var children = $(this).parent('li.parent_li').find(' > ul');
      if (children.is(":visible")) {
         children.hide();
      } else {
         children.show();
      }
   });

   // Collapse all nodes
   // http://jsfiddle.net/mehmetatas/fXzHS/2/

   $("ul[role='group']").hide();
   $('.tree > ul > li > ul').hide();

});