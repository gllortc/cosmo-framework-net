(function($) { //create closure
    $.fn.agile_carousel = function(options) {
        agile_validated = 'fail';
        validate_carousel = function() {
            var p;
            for (p in options) {
                checkme = options[p];
                checkme = checkme.toString();
                checkme = checkme.replace(/<\S+>/g, '');
            }; // for
            agile_validated = 'pass';
        }
        validate_carousel();
        if (agile_validated == 'pass') {
            slide_containter_elem = $(this);
            var defaults = {
                disable_on_first_last: "no",
                first_last_buttons: "no",
                first_slide_is_intro: "no",
                hover_next_prev_buttons: "no",
                intro_timer_length: "3000",
                intro_transtion: "no",
                next_prev_buttons: "yes",
                number_slides_visible: "1",
                pause_button: "no",
                slide_buttons: "no",
                slide_captions: "no",
                slide_directory: "slides",
                doctype: "html",
                slide_links: "no",
                slide_number_display: "no",
                timer_length: "7000",
                timer_on: "yes",
                transition_duration: 1000,
                transition_easing: "swing",
                transition_type: "carousel",
                water_mark: "no"
            };
            var opts = $.extend(defaults, options);
            $.ajax({
                type: "POST",
                url: "cs_carrousel.do",
                cache: false,
                data: {
                    first_last_buttons: defaults.first_last_buttons,
                    first_slide_is_intro: defaults.first_slide_is_intro,
                    hover_next_prev_buttons: defaults.hover_next_prev_buttons,
                    next_prev_buttons: defaults.next_prev_buttons,
                    pause_button: defaults.pause_button,
                    slide_buttons: defaults.slide_buttons,
                    slide_captions: defaults.slide_captions,
                    slide_directory: defaults.slide_directory,
                    doctype: defaults.doctype,
                    slide_links: defaults.slide_links,
                    slide_number_display: defaults.slide_number_display,
                    water_mark: defaults.water_mark
                },
                success: function(html) {
                    slide_containter_elem.html(html);

                    if (defaults.first_slide_is_intro == 'yes') {
                        intro();
                    } else {
                        carousel();
                    }
                }
            }); // ajax
            // for intro slide
            function intro() {
                $("#intro").appendTo("#slide_holder_inner");
                $('#slide_1').css("z-index", "1000");
                do_show_1 = function() {
                    $('#slide_1').show();
                };
                show_1 = setTimeout(do_show_1, 300);
                intro_transition = function() {
                    if (defaults.intro_transtion == 'fade') {
                        $('#intro').fadeOut("slow");
                        intro_timer = setTimeout(carousel, 300);
                    } else {
                        $('#intro').hide();
                    } // if
                } // function
                intro_timer = setTimeout("intro_transition()", defaults.intro_timer_length);
            }

            carousel = function() {
                // global variables (evil)
                button_class = "slide_1";
                curr_slide_id = "slide_1";
                curr_slide_id_number = 1;
                next_slide_id_number = 1;
                slideshow_paused = "not_paused";
                transition_type = defaults.transition_type;
                slide_finder = $("#slide_holder_inner div.slide");
                slide_id_array = [];
                slides_index_counter = 0;
                slide_finder.each(function() {
                    slide_id_array[slides_index_counter] = $(this).attr("id");
                    slides_index_counter++;
                });
                slide_captions_array = defaults.slide_captions.split("|");
                slide_height = $('#slide_holder_inner').height();
                half_slide_height_raw = slide_height / 2;
                half_slide_height = parseFloat(half_slide_height_raw);
                slide_holder_width = $(slide_containter_elem).width();
                slide_holder_height = $(slide_containter_elem).height();
                slide_holder_inner_width = $('#slide_holder_inner').width();
                slide_holder_inner_height = $('#slide_holder_inner').height();
                slide_holder_inner_width_px = slide_holder_inner_width + 'px';
                slide_holder_inner_height_px = slide_holder_inner_height + 'px';
                // all_slides_width_raw = slide_holder_inner_width * slide_id_array.length;
                width_per_slide = slide_holder_inner_width / defaults.number_slides_visible;
                width_per_slide = Math.floor(width_per_slide);
                all_slides_width_raw = slide_id_array.length * width_per_slide;
                all_slides_width_raw = Math.floor(all_slides_width_raw);
                all_slides_width = all_slides_width_raw + 'px';
                slide_finder_array_length = slide_finder.length;
                carousel_tranition_number_slides_visible = defaults.carousel_tranition_number_slides_visible;
                num_slides_vis = parseFloat(defaults.number_slides_visible);

                // set up the carousel
                // hide all the slides except slide_1
                $('#intro').remove();
                $('#slide_buttons').show();
                $('#slide_holder_inner div.slide:not(#slide_1)').hide();
                $('#slide_1').css("z-index", "0");
                $('#slide_holder_inner .pause_button').show();
                // initial highlighted and rollover effect for thumb images
                $("#slide_buttons li").removeAttr("id");
                $("#slide_buttons .slide_1").attr("id", "button_selected");
                // disable prev button
                if (defaults.disable_on_first_last == 'yes') {
                    $('.prev_button').attr('class', 'prev_button_disabled');
                }

                Array.prototype.inArray = function(value)
                // Returns true if the passed value is found in the
                // array. Returns false if it is not.
                {
                    var i;
                    for (i = 0; i < this.length; i++) {
                        // Matches identical (===), not just similar (==).
                        if (this[i] === value) {
                            return true;
                        }
                    }
                    return false;
                };
                slide_finder.each(function() {
                    // update slide number display
                    update_slide_number_display = function() {
                        if (defaults.slide_number_display == "yes") {
                            var id_to_split = curr_slide_id;
                            var the_currrent_slide_number_array = id_to_split.split("_");
                            var the_current_slide_number = the_currrent_slide_number_array.pop();
                            $("#slide_number_display span").html(the_current_slide_number + " of" + " <span>" + slide_id_array.length + "</span>");
                            //  $("#slide_number_display span").html(the_current_slide_number);
                        } // if
                    } // update_slide_number_display
                    if (defaults.slide_number_display == "yes") {
                        update_slide_number_display();
                    }
                    // for slide caption display
                } // change_slide_caption
                ) // each
                // pause button 
                pause = function() {
                    if (defaults.timer_on == "yes") {
                        clearInterval(slideshow_timer);
                    };
                    $("#pause_button span").html("play");
                    slideshow_paused = "paused";
                    $("#pause_button").attr("class", "paused_button");
                } // function
                change_slide_caption = function() {
                    if (defaults.slide_captions != "no") {
                        curr_caption = slide_captions_array[curr_slide_id_number - 1];
                        if (curr_caption == null) {
                            curr_caption = "";
                        }
                        $("#slide_captions span").html(curr_caption);
                    } // if
                } // if
                change_slide_caption();

                // for jquery ui effects transition
                if (defaults.transition_type == 'fold') {
                    options_object = {
                        'size': half_slide_height,
                        'easing': defaults.transition_easing
                    }
                } else {
                    options_object = {
                        'easing': defaults.transition_easing
                    };
                }
                if ((defaults.jquery_ui_effect_param != null) && (defaults.jquery_ui_effect_value != null)) {
                    jquery_ui_effect_param = defaults.jquery_ui_effect_param;
                    // handle string vs integer
                    if ((defaults.jquery_ui_effect_param == "distance") || (defaults.jquery_ui_effect_param == "number") || (defaults.jquery_ui_effect_param == "percent") || (defaults.jquery_ui_effect_param == "size") || (defaults.jquery_ui_effect_param == "times") || (defaults.jquery_ui_effect_param == "direction")) {
                        jquery_ui_effect_value = defaults.jquery_ui_effect_value;
                    } else {
                        jquery_ui_effect_value = "'" + defaults.jquery_ui_effect_value + "'";
                    }
                    options_object[jquery_ui_effect_param] = jquery_ui_effect_value;
                }
                // end for jquery ui effects transition
                // for carousel transition - position slides in a row
                if (defaults.transition_type == 'carousel') {
                    $(".slide").show();
                    for (i = 0; i < slide_finder_array_length; i++) {
                        var the_slide = slide_finder[i];
                        var x_pos = (width_per_slide) * i;
                        $(the_slide).css('left', x_pos);
                    } // for
                    $('#row_of_slides').css('width', all_slides_width);
                } // if
                // for carousel transition with multiple slides
                x_position_array = [];
                slidestop_array = [];
                sliced_slidestop_array = [];
                next_slidestop_array = [];
                prev_slidestop_array = [];

                for (i = 0; i < slide_id_array.length; i++) {
                    test_me_for_float = (i) / defaults.number_slides_visible;
                    if (i == 0 || (test_me_for_float == parseInt(test_me_for_float) && test_me_for_float == parseFloat(test_me_for_float))) {
                        var slidestop = 'yes';
                        the_x_pos = width_per_slide * (i) * -1;
                        the_next_slidestop = (1 + i + num_slides_vis);
                        if (the_next_slidestop > slide_id_array.length) {
                            the_next_slidestop = 1;
                        }
                        next_slidestop_array[i] = the_next_slidestop;
                        the_prev_slidestop = (1 + i - num_slides_vis);
                        if (the_prev_slidestop < 1) {
                            the_prev_slidestop = slide_id_array.length - num_slides_vis + 1;
                        }
                        prev_slidestop_array[i] = the_prev_slidestop;
                        next_counter = num_slides_vis - 1;
                        prev_counter = 1;
                    } else {
                        slidestop = 'no';
                        the_next_slidestop = i + 1 + next_counter;
                        if (the_next_slidestop > slide_id_array.length) {
                            the_next_slidestop = 1;
                        }
                        next_slidestop_array[i] = the_next_slidestop;
                        next_counter = next_counter + 1;
                        the_prev_slidestop = i + 1 - prev_counter;
                        if (the_prev_slidestop < 1) {
                            the_prev_slidestop == slide_id_array.length;
                        }
                        if (the_prev_slidestop > slide_id_array.length) {
                            the_prev_slidestop = 1;
                        }
                        prev_slidestop_array[i] = the_prev_slidestop;
                        prev_counter = prev_counter + 1;
                    } // if			
                    x_position_array[i] = the_x_pos;
                    slidestop_array[i] = slidestop;
                } // for
                function carousel_transition() {
                    // create array that stores desired x position per slide
                    $('#row_of_slides').stop().animate({
                        "left": x_position_array[next_slide_id_number - 1]
                    },
                    {
                        "duration": defaults.transition_duration,
                        "easing": defaults.transition_easing
                    });
                }
                // carousel transition
                function fade_transition() {
                    $('#slide_holder_inner div.slide').each(function() {
                        if ($(this).attr('id') != curr_slide_id) {
                            $(this).hide();
                        } // if
                    } // function
                    ) // each
                    $('#slide_holder_inner div[@id$="' + curr_slide_id + '"]').css("z-index", "50");
                    $('#slide_holder_inner div[@id$="' + button_class + '"]').css("z-index", "100");
                    $('#slide_holder_inner div[@id$="' + button_class + '"]').animate({
                        "opacity": "show"
                    },
                    {
                        "duration": defaults.transition_duration,
                        "easing": defaults.transition_easing
                    });
                } //function fade
                function ui_effects_transition() {
                    next_top_show_next = function() {
                        // all divs
                        $('#slide_holder_inner div.slide').show().css('z-index', '20');
                        // next div
                        $('#slide_holder_inner div[@id$="' + next_slide_id + '"]').css('z-index', '60');
                        // current div
                        $('#slide_holder_inner div[@id$="' + curr_slide_id + '"]').css('z-index', '50');
                        // the transition
                        $('#slide_holder_inner div[@id$="' + next_slide_id + '"]').stop().show(defaults.transition_type, options_object, defaults.transition_duration);
                    }
                    next_bottom_hide_curr = function() {
                        // all divs
                        $('#slide_holder_inner div.slide').show().css('z-index', '20');
                        // next div
                        var the_next_div = $('#slide_holder_inner div[@id$="' + next_slide_id + '"]').css('z-index', '50');
                        $(the_next_div).css('z-index', '1000');
                        // current div
                        $('#slide_holder_inner div[@id$="' + curr_slide_id + '"]').css('z-index', '60');
                        // the transition
                        $('#slide_holder_inner div[@id$="' + curr_slide_id + '"]').effect(defaults.transition_type, options_object, defaults.transition_duration);
                    }
                    if (defaults.transition_type == 'blind' || defaults.transition_type == 'bounce' || defaults.transition_type == 'clip' || defaults.transition_type == 'drop' || defaults.transition_type == 'fold' || defaults.transition_type == 'shake' || defaults.transition_type == 'slide' || defaults.transition_type == 'scale' || defaults.transition_type == 'pulsate') {
                        next_top_show_next();
                    } else if (defaults.transition_type == 'explode' || defaults.transition_type == 'puff') {
                        next_bottom_hide_curr();
                    } else {
                        next_top_show_next();
                    }; // if
                } //function
                // scroll right
                function scroll_right_transition() {
                    var slideshow_width = $('#slide_holder_inner').width();
                    var n_slideshow_width = -1 * slideshow_width + 'px';
                    $('#slide_holder_inner div[@id$="' + button_class + '"]').stop().show().css("left", n_slideshow_width);
                    $('#slide_holder_inner div[@id$="' + button_class + '"]').stop().show().animate({
                        "left": 0
                    },
                    {
                        'easing': defaults.transition_easing
                    },
                    defaults.transition_duration);
                    $('#slide_holder_inner div[@id$="' + curr_slide_id + '"]').stop().animate({
                        "left": slideshow_width
                    },
                    {
                        'easing': defaults.transition_easing
                    },
                    defaults.transition_duration);
                } // function
                // no effect transition
                function no_effect_transition() {
                    $('#slide_holder_inner div[@id$="' + button_class + '"]').show();
                    $('#slide_holder_inner div[@id$="' + curr_slide_id + '"]').hide();
                } // function
                function rotate_slides() {
                    function transition_slides() {
                        // transition slides
                        if (curr_slide_id != button_class) {
                            if (transition_type == 'fade') {
                                fade_transition();
                            } else if (transition_type == 'no_transition_effect') {
                                no_effect_transition();
                            } else if (transition_type == 'scroll_right') {
                                scroll_right_transition();
                            } else if (transition_type == 'carousel') {
                                carousel_transition();
                            } else if (transition_type == 'blind' || transition_type == 'clip' || transition_type == 'drop' || transition_type == 'explode' || transition_type == 'fold' || transition_type == 'puff' || transition_type == 'slide' || transition_type == 'scale' || transition_type == 'pulsate') {
                                ui_effects_transition();
                            } else {
                                no_effect_transition();
                            } // else
                        }; // if
                        curr_slide_id_number = next_slide_id_number;
                        function make_curr_slide_id() {
                            curr_slide_id = button_class;
                        };
                        make_curr_slide_id();
                    } // transition_slides()
                    transition_slides();

                    function animate_slides() {} // animate_slides()
                } // rotate_slides()
                function change_button_class(button_class) {
                    $("#slide_buttons li").removeAttr("id");
                    $("#slide_buttons li").each(function() {
                        if ($(this).attr("class") == button_class) {
                            $(this).attr("id", "button_selected");
                        } // if
                    } // function
                    ); // each
                } // change_button_class
                // functions for pause button
                $(".pause_button").click(function() {
                    if (slideshow_paused == "paused") {
                        // skip to next slide
                        skip('next');
                        if (defaults.timer_on == "yes") {
                            slideshow_timer = setInterval("skip('next')", defaults.timer_length);
                        } //if
                        slideshow_paused = "not_paused";
                        $("#pause_button span").html("pause");
                        $(this).attr("class", "pause_button");
                    } else if (slideshow_paused == "not_paused") {
                        clearInterval(slideshow_timer);
                        slideshow_paused = "paused";
                        $(this).attr("class", "paused_button");
                        pause();
                    }
                } // function
                ) // click
                skip = function(direction) {
                    // identify next slide class
                    curr_slide_id_string = curr_slide_id.toString();
                    split_curr_slide_id_string = curr_slide_id.split("_");
                    curr_slide_id_string = split_curr_slide_id_string.pop();
                    curr_slide_id_number = parseFloat(curr_slide_id_string);
                    if (direction == 'next') {
                        next_slide_id_number = curr_slide_id_number + 1;
                        if (defaults.transition_type == 'carousel') {
                            the_index_to_use = curr_slide_id_number - 1;
                            next_slide_id_number = next_slidestop_array[the_index_to_use];
                        }
                    } else if (direction == 'prev') {
                        next_slide_id_number = curr_slide_id_number - 1;
                        if (defaults.transition_type == 'carousel') {
                            the_index_to_use = curr_slide_id_number - 1;
                            next_slide_id_number = prev_slidestop_array[the_index_to_use];
                        }
                    } else if (direction == 'first') {
                        next_slide_id_number = slide_id_array.length;
                        curr_slide_id_number = 1;
                    } else if (direction == 'last') {
                        next_slide_id_number = 1;
                        curr_slide_id_number = slide_id_array.length;
                    } else {
                        next_slide_id_number = direction;
                    };
                    next_slide_id = "slide_" + next_slide_id_number;
                    // if next slide is after the last slide, then go to first slide
                    if (next_slide_id_number > slide_id_array.length) {
                        next_slide_id = "slide_1";
                        curr_slide_id_number = slide_id_array.length;
                        next_slide_id_number = 1;
                        // if next slide is before the first slide, then go to first slide	
                    } else if (next_slide_id_number < 1) {
                        next_slide_id = "slide_" + slide_id_array.length;
                        next_slide_id_number = slide_id_array.length;
                    } // else 
                    button_class = next_slide_id;
                    // animate to next slide
                    rotate_slides();
                    change_button_class(button_class);
                    if (defaults.slide_number_display == "yes") {
                        update_slide_number_display();
                    }
                    change_slide_caption();
                    // disable first and last buttons
                    if ((defaults.disable_on_first_last == 'yes' && ((next_slide_id_number == slide_id_array.length) || (next_slide_id_number == slide_id_array.length - num_slides_vis + 1)))) {
                        $('#next_button').attr('class', 'next_button_disabled');
                        $('#hover_next_button').attr('class', 'hover_next_buttondisabled');
                        $('#last_button').attr('class', 'last_buttondisabled');
                    } else {
                        $('#next_button').attr('class', 'next_button');
                        $('#hover_next_button').attr('class', 'hover_next_button');
                        $('#last_button').attr('class', 'last_buton');
                    }

                    if ((defaults.disable_on_first_last == 'yes' && ((next_slide_id_number == 1) || (next_slide_id_number == num_slides_vis)))) {
                        $('#prev_button').attr('class', 'prev_button_disabled');
                        $('#hover_prev_button').attr('class', 'hover_prev_button_disabled');
                        $('#first_button').attr('class', 'first_button_disabled');
                    } else {
                        $('#prev_button').attr('class', 'prev_button');
                        $('#hover_prev_button').attr('class', 'hover_prev_button');
                        $('#first_button').attr('class', 'first_button');
                    }
                } //skip
                if (defaults.timer_on == "yes") {
                    slideshow_timer = setInterval("skip('next')", defaults.timer_length);
                } // if
                // functions for clicking slide buttons
                $("#slide_buttons li").each(function() {
                    // add click functionality to buttons
                    $(this).click(function() {
                        button_class = $(this).attr("class");
                        change_button_class(button_class);
                        // declare next_slide_id_number
                        split_button_class_string = button_class.split("_");
                        button_class_string = split_button_class_string.pop();
                        next_slide_id_number = parseFloat(button_class_string);
                        pause();
                        skip(next_slide_id_number);
                        return (false);
                    }); // click
                }); //each
                // remove unused slide buttons for carousel transtion
                if (defaults.transition_type == 'carousel') {
                    $("#slide_buttons li").each(function() {
                        test_the_button_number = $(this).text() - 1;
                        if (slidestop_array[test_the_button_number] == "no") {
                            $(this).remove();
                        } // if
                    } // function
                    ) // each
                } // if
                // functions for clicking prev & next buttons
                // next button
                $(".next_button").click(function() {
                    if (defaults.disable_on_first_last != 'yes') {
                        skip('next');
                        pause();
                    } else if ((defaults.disable_on_first_last == 'yes' && next_slide_id_number == slide_id_array.length) || (next_slide_id_number == (slide_id_array.length - num_slides_vis + 1))) {
                        pause();
                    } else {
                        skip('next');
                        pause();
                    }
                    // if
                } // function
                ); // click	
                $(".prev_button, .prev_button_disabled").click(function() {
                    if (defaults.disable_on_first_last != 'yes') {
                        skip('prev');
                        pause();
                    } else if ((defaults.disable_on_first_last == 'yes' && curr_slide_id_number == 1) || (defaults.disable_on_first_last == 'yes' && next_slide_id_number == num_slides_vis)) {
                        pause();
                    } else {
                        skip('prev');
                        pause();
                    }
                    // if
                } // function
                ); // click	
                if (defaults.hover_next_prev_buttons == "yes") {
                    $(".hover_button").fadeTo(1, 0,
                    function() {
                        $(this).css("display", "block")
                    });
                    $('#hover_prev_button').hover(function() {
                        $("#hover_prev_button").stop().fadeTo("slow", 0.95);
                    },
                    // function
                    function() {
                        $("#hover_prev_button").stop().fadeTo("slow", 0.00);
                    }); // hover
                    $('#hover_next_button').hover(function() {
                        $("#hover_next_button").stop().fadeTo("slow", 0.95);
                    },
                    // function
                    function() {
                        $("#hover_next_button").stop().fadeTo("slow", 0.00);
                    }); // hover
                } // if
                $("#first_button").click(function() {
                    skip('last');
                    pause();
                } // function
                ); // click	
                $("#last_button").click(function() {
                    pause();
                    skip('first');
                } // function
                ); // click	
            } // carousel
        } // if (validation)
    } // function agile_carousel
})(jQuery);