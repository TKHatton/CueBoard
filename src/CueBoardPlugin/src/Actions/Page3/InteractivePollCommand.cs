namespace Loupedeck.CueBoardPlugin.Actions.Page3
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;

    public class InteractivePollCommand : CueBoardCommand
    {
        public InteractivePollCommand()
            : base("Interactive Poll", "Open polls, word cloud, and feedback tools", "Meeting Intelligence")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            try
            {
                var html = GenerateEngagementPage();

                var folder = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "CueBoard");
                Directory.CreateDirectory(folder);

                var filePath = Path.Combine(folder, "engage.html");
                File.WriteAllText(filePath, html, Encoding.UTF8);

                try
                {
                    Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
                }
                catch
                {
                    Process.Start(new ProcessStartInfo("cmd.exe", $"/c start \"\" \"{filePath}\"")
                    {
                        CreateNoWindow = true,
                        UseShellExecute = false
                    });
                }

                this.CueBoard?.Toast?.ShowToast("\uD83D\uDCCA", "Engagement tools launched", 2000);
                PluginLog.Info($"Engagement page opened: {filePath}");
            }
            catch (Exception ex)
            {
                PluginLog.Error(ex, "Failed to open engagement page");
            }
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            return this.DrawIcon(imageSize, "poll.png");
        }

        private static String GenerateEngagementPage()
        {
            return @"<!DOCTYPE html>
<html lang='en'>
<head>
<meta charset='UTF-8'>
<meta name='viewport' content='width=device-width, initial-scale=1.0'>
<title>CueBoard Engage</title>
<style>
*{margin:0;padding:0;box-sizing:border-box}
body{font-family:'Segoe UI',-apple-system,sans-serif;background:#0f0f13;color:#e0e0e0;min-height:100vh}
.container{max-width:640px;margin:0 auto;padding:24px 20px}

/* Header */
.header{text-align:center;margin-bottom:20px}
.logo{font-size:11px;font-weight:700;letter-spacing:3px;color:#8B5CF6;text-transform:uppercase;margin-bottom:4px}
.title{font-size:22px;font-weight:700;color:#fff}

/* Tabs */
.tabs{display:flex;gap:3px;margin-bottom:20px;background:#1a1a24;border-radius:12px;padding:3px}
.tab{flex:1;padding:10px 8px;text-align:center;border-radius:10px;cursor:pointer;font-size:13px;font-weight:600;color:#666;transition:all .2s}
.tab:hover{color:#aaa}
.tab.active{background:#8B5CF6;color:#fff}

.panel{display:none}.panel.active{display:block}

/* Inputs */
input[type='text'],input[type='email'],textarea{width:100%;background:#1a1a24;border:1px solid #2a2a35;color:#fff;border-radius:10px;padding:12px 16px;font-size:15px;font-family:inherit;transition:border .2s}
input:focus,textarea:focus{outline:none;border-color:#8B5CF6}
::placeholder{color:#555}
textarea{resize:vertical;min-height:100px;line-height:1.6}

.btn{padding:12px 24px;border-radius:10px;border:none;font-size:14px;font-weight:700;cursor:pointer;transition:all .2s;font-family:inherit}
.btn-primary{background:#8B5CF6;color:#fff;width:100%}
.btn-primary:hover{background:#7C3AED}
.btn-primary:active{transform:scale(.98)}
.btn-sm{padding:8px 16px;font-size:12px;width:auto}
.btn-ghost{background:#2a2a35;color:#ccc}.btn-ghost:hover{background:#353545}
.gap-8{margin-bottom:8px}.gap-12{margin-bottom:12px}.gap-16{margin-bottom:16px}.gap-20{margin-bottom:20px}.gap-24{margin-bottom:24px}
.row{display:flex;gap:8px}
.center{text-align:center}
.muted{color:#888;font-size:13px}
.label{display:block;font-size:12px;color:#999;margin-bottom:6px;font-weight:600;text-transform:uppercase;letter-spacing:.5px}

/* Word Cloud */
.cloud-box{background:#1a1a24;border-radius:16px;padding:28px;min-height:240px;display:flex;flex-wrap:wrap;align-items:center;justify-content:center;gap:10px}
.cloud-word{display:inline-block;padding:3px 10px;border-radius:8px;transition:transform .2s}
.cloud-word:hover{transform:scale(1.12)}
.cloud-empty{color:#333;font-size:15px;font-style:italic}

/* Poll */
.poll-q{font-size:18px;font-weight:700;color:#fff;text-align:center;padding:16px;background:#1a1a24;border-radius:12px}
.poll-opts{display:flex;gap:12px;justify-content:center}
.poll-opt{flex:1;max-width:160px;padding:20px 12px;background:#1a1a24;border-radius:14px;text-align:center;cursor:pointer;transition:all .2s;border:2px solid transparent}
.poll-opt:hover{border-color:#8B5CF6;transform:scale(1.03)}
.poll-opt.voted{border-color:#8B5CF6;background:#1a1528}
.poll-emoji{font-size:32px;margin-bottom:6px}
.poll-lbl{font-size:15px;font-weight:600}
.poll-num{font-size:22px;font-weight:700;color:#8B5CF6;margin-top:6px}
.bar-wrap{margin-bottom:12px}
.bar-head{display:flex;justify-content:space-between;font-size:13px;margin-bottom:3px}
.bar-bg{background:#1a1a24;border-radius:6px;height:28px;overflow:hidden}
.bar-fill{height:100%;border-radius:6px;transition:width .5s;display:flex;align-items:center;padding-left:10px;font-size:12px;font-weight:600;color:#fff}

/* Rating */
.rating-big{text-align:center;padding:24px;background:#1a1a24;border-radius:16px}
.rating-num{font-size:56px;font-weight:700;color:#F59E0B}
.rating-stars-lg{font-size:28px;margin:6px 0}
.star-input{display:flex;gap:6px;justify-content:center}
.star-btn{font-size:36px;cursor:pointer;color:#2a2a35;transition:color .15s;user-select:none}
.star-btn:hover,.star-btn.on{color:#F59E0B}
.bar-row{display:flex;align-items:center;gap:8px;margin-bottom:5px;font-size:12px}
.bar-row .bg{flex:1;background:#1a1a24;border-radius:4px;height:10px;overflow:hidden}
.bar-row .fl{height:100%;background:#F59E0B;border-radius:4px;transition:width .5s}

/* Testimonial */
.testi-card{background:#1a1a24;border-radius:16px;padding:24px}
.testi-stars{display:flex;gap:4px;justify-content:center;margin-bottom:16px}
.testi-star{font-size:32px;cursor:pointer;color:#2a2a35;transition:color .15s;user-select:none}
.testi-star:hover,.testi-star.on{color:#F59E0B}
.permission{display:flex;gap:16px;margin-top:8px}
.permission label{display:flex;align-items:center;gap:6px;cursor:pointer;font-size:14px;color:#ccc}
.permission input{width:auto}
.success-view{display:none;text-align:center;padding:40px 20px}
.success-icon{font-size:48px;margin-bottom:12px}
.success-title{font-size:20px;font-weight:700;color:#fff;margin-bottom:6px}

.coming-soon{text-align:center;padding:20px;background:#1a1a24;border-radius:12px;margin-top:16px}
.coming-tag{display:inline-block;background:#8B5CF6;color:#fff;font-size:10px;font-weight:700;padding:3px 10px;border-radius:20px;letter-spacing:1px;text-transform:uppercase;margin-bottom:8px}

.footer{text-align:center;margin-top:28px;padding-top:14px;border-top:1px solid #1a1a24;font-size:10px;color:#333}

@media(max-width:500px){
  .poll-opts{flex-direction:column;align-items:center}
  .poll-opt{max-width:100%;width:100%}
}
</style>
</head>
<body>
<div class='container'>
  <div class='header'>
    <div class='logo'>CueBoard</div>
    <div class='title'>Engage</div>
  </div>

  <div class='tabs'>
    <div class='tab active' onclick='go(""cloud"")'>Word Cloud</div>
    <div class='tab' onclick='go(""poll"")'>Poll</div>
    <div class='tab' onclick='go(""rate"")'>Rate</div>
    <div class='tab' onclick='go(""feedback"")'>Feedback</div>
  </div>

  <!-- ===== WORD CLOUD ===== -->
  <div class='panel active' id='p-cloud'>
    <div class='cloud-box gap-16' id='cloud-display'>
      <span class='cloud-empty'>Add words to build the cloud</span>
    </div>
    <div class='row gap-12'>
      <input type='text' id='cloud-in' placeholder='Type a word and press Enter'/>
      <button class='btn btn-primary btn-sm' onclick='addWord()' style='width:80px'>Add</button>
    </div>
    <div class='center'>
      <button class='btn btn-ghost btn-sm' onclick='demoCloud()'>Demo Data</button>
      <button class='btn btn-ghost btn-sm' onclick='cloudWords={};renderCloud()'>Clear</button>
    </div>
  </div>

  <!-- ===== POLL ===== -->
  <div class='panel' id='p-poll'>
    <!-- Poll type selector -->
    <div class='row gap-12' style='justify-content:center'>
      <button class='btn btn-ghost btn-sm' id='pt-yn' onclick='setPollType(""yn"")' style='border:2px solid #8B5CF6;color:#fff'>Yes / No</button>
      <button class='btn btn-ghost btn-sm' id='pt-tf' onclick='setPollType(""tf"")'>True / False</button>
      <button class='btn btn-ghost btn-sm' id='pt-mc' onclick='setPollType(""mc"")'>Multiple Choice</button>
    </div>

    <input type='text' id='poll-q-input' placeholder='Type your question...' value='Do you agree with this approach?' class='gap-16' style='text-align:center;font-size:17px;font-weight:600'/>

    <!-- Yes/No and True/False options -->
    <div id='poll-fixed-opts'>
      <div class='poll-opts gap-16' id='poll-opts-container'></div>
    </div>

    <!-- Multiple Choice: custom options -->
    <div id='poll-mc-setup' style='display:none' class='gap-16'>
      <div id='mc-options-list'></div>
      <div class='row gap-12'>
        <input type='text' id='mc-new-opt' placeholder='Add an option and press Enter'/>
        <button class='btn btn-primary btn-sm' onclick='addMcOption()' style='width:80px'>Add</button>
      </div>
    </div>

    <div id='poll-bars' class='gap-12'></div>
    <div class='center'>
      <button class='btn btn-ghost btn-sm' onclick='demoPoll()'>Demo</button>
      <button class='btn btn-ghost btn-sm' onclick='resetPoll()'>Reset</button>
    </div>
  </div>

  <!-- ===== RATING ===== -->
  <div class='panel' id='p-rate'>
    <input type='text' placeholder='What are you rating?' value='How would you rate this session?' class='gap-16' style='text-align:center;font-size:17px;font-weight:600'/>
    <div class='rating-big gap-16'>
      <div class='rating-num' id='r-avg'>0.0</div>
      <div class='rating-stars-lg' id='r-stars'>&#9734;&#9734;&#9734;&#9734;&#9734;</div>
      <div class='muted' id='r-total'>0 responses</div>
    </div>
    <div class='center muted gap-8'>Cast your vote:</div>
    <div class='star-input gap-16' id='star-in'></div>
    <div id='r-bars' style='max-width:360px;margin:0 auto' class='gap-12'></div>
    <div class='center'>
      <button class='btn btn-ghost btn-sm' onclick='rv=[1,2,4,8,12];renderRate()'>Demo</button>
      <button class='btn btn-ghost btn-sm' onclick='rv=[0,0,0,0,0];renderRate()'>Reset</button>
    </div>
  </div>

  <!-- ===== FEEDBACK / TESTIMONIAL ===== -->
  <div class='panel' id='p-feedback'>
    <div id='fb-form'>
      <div class='testi-card'>
        <div class='center muted gap-8'>How was your experience?</div>
        <div class='testi-stars gap-16' id='fb-stars'></div>

        <div class='gap-12'>
          <label class='label'>Your Name</label>
          <input type='text' id='fb-name' placeholder='How you want to be credited'/>
        </div>

        <div class='gap-12'>
          <label class='label'>Your Email (optional)</label>
          <input type='email' id='fb-email' placeholder='So we can follow up or say thanks'/>
        </div>

        <div class='gap-16'>
          <label class='label'>Your Feedback</label>
          <textarea id='fb-text' placeholder='What stood out? What was most valuable? Would you recommend this to others?'></textarea>
        </div>

        <div class='gap-16'>
          <label class='label'>Permission</label>
          <div class='permission'>
            <label><input type='radio' name='perm' value='yes' checked/> Use my name</label>
            <label><input type='radio' name='perm' value='anon'/> Keep anonymous</label>
          </div>
        </div>

        <button class='btn btn-primary' onclick='submitFb()'>Submit Feedback</button>
      </div>

      <div class='coming-soon'>
        <div class='coming-tag'>Coming Soon</div>
        <div class='muted'>Audio and video testimonials</div>
      </div>
    </div>

    <div class='success-view' id='fb-success'>
      <div class='success-icon'>&#10024;</div>
      <div class='success-title'>Thank you!</div>
      <div class='muted'>Your feedback has been saved. It means more than you know.</div>
    </div>
  </div>

  <div class='footer'>Powered by CueBoard</div>
</div>

<script>
var tabs=['cloud','poll','rate','feedback'];
function go(t){
  document.querySelectorAll('.tab').forEach(function(el,i){el.classList.toggle('active',tabs[i]===t)});
  document.querySelectorAll('.panel').forEach(function(p){p.classList.remove('active')});
  document.getElementById('p-'+t).classList.add('active');
}

// ===== WORD CLOUD =====
var cloudWords={};
var cc=['#8B5CF6','#3498DB','#2ECC71','#F59E0B','#E74C3C','#0D9488','#EC4899','#F97316','#06B6D4','#84CC16'];
document.getElementById('cloud-in').addEventListener('keydown',function(e){if(e.key==='Enter'){e.preventDefault();addWord()}});
function addWord(){var i=document.getElementById('cloud-in'),w=i.value.trim().toLowerCase();if(!w)return;cloudWords[w]=(cloudWords[w]||0)+1;i.value='';renderCloud()}
function renderCloud(){
  var d=document.getElementById('cloud-display'),e=Object.entries(cloudWords).sort(function(a,b){return b[1]-a[1]});
  if(!e.length){d.innerHTML='<span class=""cloud-empty"">Add words to build the cloud</span>';return}
  var mx=e[0][1],sh=e.slice().sort(function(){return Math.random()-.5}),h='';
  sh.forEach(function(x,i){var sz=16+Math.round((x[1]/mx)*40),op=.6+(x[1]/mx)*.4;
    h+='<span class=""cloud-word"" style=""font-size:'+sz+'px;color:'+cc[i%cc.length]+';opacity:'+op+'"" title=""'+x[0]+': '+x[1]+'"">'+x[0]+'</span> '});
  d.innerHTML=h}
function demoCloud(){cloudWords={collaboration:8,innovation:6,teamwork:5,growth:7,strategy:4,feedback:5,leadership:3,agile:4,vision:6,execution:3,alignment:5,impact:7,culture:4,accountability:3,transparency:5,empowerment:2,scalable:3,resilience:4,mindset:6,inclusive:3};renderCloud()}

// ===== POLL =====
var pollType='yn';
var pollData={};
var pollColors=['#8B5CF6','#2ECC71','#E74C3C','#F59E0B','#3498DB','#EC4899','#0D9488','#F97316'];
var pollPresets={
  yn:[{key:'y',label:'Yes',emoji:'&#128077;'},{key:'n',label:'No',emoji:'&#128078;'},{key:'m',label:'Not Sure',emoji:'&#129300;'}],
  tf:[{key:'t',label:'True',emoji:'&#9989;'},{key:'f',label:'False',emoji:'&#10060;'}]
};
var mcOptions=[];

function setPollType(t){
  pollType=t;pollData={};
  document.querySelectorAll('[id^=""pt-""]').forEach(function(b){b.style.border='2px solid transparent';b.style.color='#ccc'});
  document.getElementById('pt-'+t).style.border='2px solid #8B5CF6';document.getElementById('pt-'+t).style.color='#fff';
  document.getElementById('poll-fixed-opts').style.display=t==='mc'?'none':'block';
  document.getElementById('poll-mc-setup').style.display=t==='mc'?'block':'none';
  if(t!=='mc'){renderFixedOpts()}else{mcOptions=[];renderMcOpts()}
  document.getElementById('poll-bars').innerHTML='';
}

function renderFixedOpts(){
  var opts=pollPresets[pollType]||pollPresets.yn;
  var h='';opts.forEach(function(o){pollData[o.key]=0;
    h+='<div class=""poll-opt"" onclick=""voteKey(\''+o.key+'\')\""><div class=""poll-emoji"">'+o.emoji+'</div><div class=""poll-lbl"">'+o.label+'</div><div class=""poll-num"" id=""pv-'+o.key+'\"">0</div></div>'});
  document.getElementById('poll-opts-container').innerHTML=h;
}

function voteKey(k){pollData[k]=(pollData[k]||0)+1;var el=document.getElementById('pv-'+k);if(el)el.textContent=pollData[k];renderBars()}

document.getElementById('mc-new-opt').addEventListener('keydown',function(e){if(e.key==='Enter'){e.preventDefault();addMcOption()}});
function addMcOption(){
  var input=document.getElementById('mc-new-opt'),val=input.value.trim();if(!val||mcOptions.length>=8)return;
  mcOptions.push(val);pollData[val]=0;input.value='';renderMcOpts()}
function renderMcOpts(){
  var h='<div class=""poll-opts gap-16"" style=""flex-wrap:wrap"">';
  mcOptions.forEach(function(o,i){
    h+='<div class=""poll-opt"" onclick=""voteMc(\''+o.replace(/'/g,""\\'"")+'\')"" style=""min-width:120px""><div class=""poll-emoji"" style=""font-size:24px;color:'+pollColors[i%pollColors.length]+'"">&#9679;</div><div class=""poll-lbl"">'+o+'</div><div class=""poll-num"" id=""pv-mc-'+i+'\"">0</div></div>'});
  h+='</div>';document.getElementById('poll-mc-setup').innerHTML=h+
    '<div class=""row gap-12""><input type=""text"" id=""mc-new-opt"" placeholder=""Add an option and press Enter""/><button class=""btn btn-primary btn-sm"" onclick=""addMcOption()"" style=""width:80px"">Add</button></div>';
  document.getElementById('mc-new-opt').addEventListener('keydown',function(e){if(e.key==='Enter'){e.preventDefault();addMcOption()}});
}
function voteMc(o){pollData[o]=(pollData[o]||0)+1;var i=mcOptions.indexOf(o);var el=document.getElementById('pv-mc-'+i);if(el)el.textContent=pollData[o];renderBars()}

function renderBars(){
  var entries=Object.entries(pollData).filter(function(e){return e[1]>0||pollType==='mc'||pollPresets[pollType]});
  var t=entries.reduce(function(a,e){return a+e[1]},0);if(!t){document.getElementById('poll-bars').innerHTML='';return}
  var h='';entries.forEach(function(e,i){var lbl=e[0],cnt=e[1],p=Math.round(cnt/t*100);
    // Map keys to labels for presets
    if(pollPresets[pollType]){var found=pollPresets[pollType].find(function(x){return x.key===lbl});if(found)lbl=found.label}
    h+='<div class=""bar-wrap""><div class=""bar-head""><span>'+lbl+'</span><span>'+p+'%</span></div><div class=""bar-bg""><div class=""bar-fill"" style=""width:'+p+'%;background:'+pollColors[i%pollColors.length]+'"">'+cnt+'</div></div></div>'});
  document.getElementById('poll-bars').innerHTML=h}

function demoPoll(){
  if(pollType==='mc'){mcOptions=['React','Vue','Angular','Svelte'];pollData={React:12,Vue:8,Angular:5,Svelte:9};renderMcOpts()}
  else if(pollType==='tf'){pollData={t:18,f:7};renderFixedOpts()}
  else{pollData={y:14,n:3,m:5};renderFixedOpts()}
  Object.keys(pollData).forEach(function(k){var el=document.getElementById('pv-'+k);if(el)el.textContent=pollData[k]});
  renderBars()}
function resetPoll(){pollData={};if(pollType==='mc'){mcOptions=[];renderMcOpts()}else{renderFixedOpts()};document.getElementById('poll-bars').innerHTML=''}

// ===== RATING =====
var rv=[0,0,0,0,0];
function initRate(){
  var c=document.getElementById('star-in'),h='';
  for(var i=1;i<=5;i++)h+='<span class=""star-btn"" onclick=""castRate('+i+')"" onmouseenter=""hoverRate('+i+')"" onmouseleave=""hoverRate(0)"">&#9733;</span>';
  c.innerHTML=h;renderRate()}
function castRate(v){rv[v-1]++;renderRate()}
function hoverRate(v){document.querySelectorAll('.star-btn').forEach(function(s,i){s.style.color=i<v?'#F59E0B':'#2a2a35'})}
function renderRate(){
  var t=rv.reduce(function(a,b){return a+b},0),sm=rv.reduce(function(a,b,i){return a+b*(i+1)},0),av=t?(sm/t).toFixed(1):'0.0';
  document.getElementById('r-avg').textContent=av;document.getElementById('r-total').textContent=t+' response'+(t!==1?'s':'');
  var st='',an=parseFloat(av);for(var i=1;i<=5;i++)st+=i<=Math.round(an)?'&#9733;':'&#9734;';
  document.getElementById('r-stars').innerHTML=st;
  var h='';for(var i=4;i>=0;i--){var p=t?Math.round(rv[i]/t*100):0;
    h+='<div class=""bar-row""><span>'+(i+1)+' &#9733;</span><div class=""bg""><div class=""fl"" style=""width:'+p+'%""></div></div><span>'+rv[i]+'</span></div>'}
  document.getElementById('r-bars').innerHTML=h}

// ===== FEEDBACK =====
var fbRating=0;
function initFb(){
  var c=document.getElementById('fb-stars'),h='';
  for(var i=1;i<=5;i++)h+='<span class=""testi-star"" data-v=""'+i+'"" onclick=""setFbRating('+i+')"" onmouseenter=""hoverFb('+i+')"" onmouseleave=""hoverFb(0)"">&#9733;</span>';
  c.innerHTML=h}
function setFbRating(v){fbRating=v;document.querySelectorAll('.testi-star').forEach(function(s,i){s.classList.toggle('on',i<v)})}
function hoverFb(v){document.querySelectorAll('.testi-star').forEach(function(s,i){s.style.color=i<v?'#F59E0B':i<fbRating?'#F59E0B':'#2a2a35'})}
function submitFb(){
  var name=document.getElementById('fb-name').value.trim();
  var email=document.getElementById('fb-email').value.trim();
  var text=document.getElementById('fb-text').value.trim();
  var perm=document.querySelector('input[name=""perm""]:checked').value;
  if(!text){document.getElementById('fb-text').style.borderColor='#E74C3C';return}
  var all=JSON.parse(localStorage.getItem('cueboard_feedback')||'[]');
  var entry={name:name||'Anonymous',email:email,rating:fbRating,text:text,permission:perm,date:new Date().toISOString()};
  all.push(entry);localStorage.setItem('cueboard_feedback',JSON.stringify(all));
  // Download backup
  var blob=new Blob([JSON.stringify(entry,null,2)],{type:'application/json'});
  var a=document.createElement('a');a.href=URL.createObjectURL(blob);
  a.download='feedback-'+(name||'anon').replace(/[^a-zA-Z0-9]/g,'_')+'.json';a.click();
  document.getElementById('fb-form').style.display='none';
  document.getElementById('fb-success').style.display='block'}

initRate();initFb();setPollType('yn');
</script>
</body>
</html>";
        }
    }
}
