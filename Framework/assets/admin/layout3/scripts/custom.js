
function Idle(link,timeout)
{
	if(timeout==0)
		return;
		timeout = (timeout*60)-3;
	 // cache a reference to the countdown element so we don't have to query the DOM for it on each ping.
            var $countdown;
            // start the idle timer plugin
            $.idleTimeout('#idle-timeout-dialog', '.modal-content button:last', {
                idleAfter: (timeout/2),
                timeout: timeout, 
                pollingInterval: 60,
                keepAliveURL: link + 'login/keepalive',
                serverResponseEquals: 'OK',
                onTimeout: function(){
                    window.location = link + "logout";
                },
                onIdle: function(){ 
                    $('#idle-timeout-dialog').modal('show');
                    $countdown = $('#idle-timeout-counter');

                    $('#idle-timeout-dialog-keepalive').on('click', function () { 
                        $('#idle-timeout-dialog').modal('hide');
                    });

                    $('#idle-timeout-dialog-logout').on('click', function () { 
                        $('#idle-timeout-dialog').modal('hide');
                        $.idleTimeout.options.onTimeout.call(this);
                    });
                },
                onCountdown: function(counter){
                    $countdown.html(counter); // update the counter
                }
            });
}
