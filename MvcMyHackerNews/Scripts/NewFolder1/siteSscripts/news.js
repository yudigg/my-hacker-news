$(function () {
   
    $('.upvote').on('click', function () {
        var linkId = $(this).data('id');
        var userId = $(this).data('userid');
        var btn = $(this);
        $.post('/homePage/Upvote', { linkId: linkId, userId: userId }, function (model) {

            if(model.IsAdded)
            {             
                $(btn).hide();
                $(btn).parent().find('span').text(model.Points);
            }
        })
    })
})