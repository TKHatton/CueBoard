namespace Loupedeck.CueBoardPlugin.Actions.Page3
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Pre-meeting setup for the Interactive Poll engagement page. Opens a config form
    /// in the browser that writes word cloud prompt, poll question queue, and rating
    /// prompt to localStorage. The Engage page reads from the same localStorage.
    ///
    /// Workflow: hit Engage Setup before the meeting -> fill in questions -> Save.
    /// During the meeting, hit Interactive Poll -> link auto-copied -> paste in Zoom chat.
    /// Attendees open the link, Engage page picks up the preset from localStorage.
    /// </summary>
    public class EngageSetupCommand : CueBoardCommand
    {
        public EngageSetupCommand()
            : base("Engage Setup", "Pre-meeting: configure word cloud + poll questions", "Meeting Intelligence")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            try
            {
                var folder = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "CueBoard");
                Directory.CreateDirectory(folder);

                var filePath = Path.Combine(folder, "setup.html");
                File.WriteAllText(filePath, GenerateSetupPage(), Encoding.UTF8);

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

                this.CueBoard?.Toast?.ShowToast("⚙", "Setup page opened", 2000);
                PluginLog.Info($"Engage setup opened: {filePath}");
            }
            catch (Exception ex)
            {
                PluginLog.Error(ex, "Failed to open setup page");
            }
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            return this.DrawButton(imageSize, "ENGAGE\nSETUP", new BitmapColor(99, 102, 241));
        }

        private static String GenerateSetupPage()
        {
            return @"<!DOCTYPE html>
<html lang='en'><head><meta charset='UTF-8'><title>CueBoard Engage Setup</title>
<style>
*{margin:0;padding:0;box-sizing:border-box}
body{font-family:'Segoe UI',-apple-system,sans-serif;background:#0f0f13;color:#e0e0e0;min-height:100vh}
.container{max-width:680px;margin:0 auto;padding:32px 20px}
.header{text-align:center;margin-bottom:24px}
.logo{font-size:11px;font-weight:700;letter-spacing:3px;color:#8B5CF6;text-transform:uppercase}
.title{font-size:24px;font-weight:700;color:#fff;margin-top:4px}
.sub{font-size:13px;color:#888;margin-top:6px}
.card{background:#1a1a24;border-radius:14px;padding:22px;margin-bottom:18px}
.card h3{font-size:13px;color:#8B5CF6;text-transform:uppercase;letter-spacing:2px;margin-bottom:14px;font-weight:700}
input[type=text],select,textarea{width:100%;background:#252530;border:1px solid #2a2a35;color:#fff;border-radius:8px;padding:10px 14px;font-size:14px;font-family:inherit}
input:focus,textarea:focus,select:focus{outline:none;border-color:#8B5CF6}
::placeholder{color:#555}
.label{display:block;font-size:11px;color:#999;margin-bottom:6px;font-weight:600;text-transform:uppercase;letter-spacing:1px}
.btn{padding:11px 22px;border-radius:9px;border:none;font-size:14px;font-weight:700;cursor:pointer;transition:all .2s;font-family:inherit}
.btn-primary{background:#8B5CF6;color:#fff}.btn-primary:hover{background:#7C3AED}
.btn-ghost{background:#2a2a35;color:#ccc}.btn-ghost:hover{background:#353545}
.btn-sm{padding:7px 14px;font-size:12px}
.q-row{display:flex;gap:8px;align-items:flex-start;margin-bottom:10px;padding:12px;background:#22222e;border-radius:10px}
.q-row .num{font-size:11px;color:#666;font-weight:700;padding-top:8px;width:18px}
.q-row .body{flex:1}
.q-row select{width:auto;font-size:12px;padding:6px 8px;margin-bottom:6px}
.q-row textarea{min-height:42px;font-size:13px;line-height:1.4;margin-bottom:6px;resize:vertical}
.q-row .opts{font-size:11px;color:#888;font-style:italic}
.q-row .del{background:transparent;color:#666;border:none;cursor:pointer;font-size:18px;padding:6px}
.q-row .del:hover{color:#E74C3C}
.actions{display:flex;gap:10px;justify-content:center;margin-top:20px}
.success{display:none;text-align:center;padding:32px;background:#152028;border-radius:12px;color:#2ECC71;font-weight:600;margin-bottom:20px}
.tip{font-size:12px;color:#666;margin-top:8px;line-height:1.5}
</style></head><body>
<div class='container'>
<div class='header'><div class='logo'>CueBoard</div><div class='title'>Engage Setup</div><div class='sub'>Pre-meeting · Configure questions before you start</div></div>

<div id='success' class='success'>✓ Saved. Now press Interactive Poll on your CueBoard to launch the live page.</div>

<div class='card'>
<h3>Word Cloud Prompt</h3>
<input type='text' id='wc-prompt' placeholder='What is one word for...?'/>
<div class='tip'>Shown at the top of the Word Cloud tab so attendees know what to type.</div>
</div>

<div class='card'>
<h3>Poll Questions <span style='float:right;font-weight:400;text-transform:none;letter-spacing:0'><button class='btn btn-ghost btn-sm' onclick='addQ()'>+ Add Question</button></span></h3>
<div id='q-list'></div>
<div class='tip'>During the meeting use the Next Question button on the live page to cycle through.</div>
</div>

<div class='card'>
<h3>Rating Prompt</h3>
<input type='text' id='rate-prompt' placeholder='How would you rate this session?'/>
</div>

<div class='actions'>
<button class='btn btn-ghost' onclick='clearAll()'>Clear All</button>
<button class='btn btn-primary' onclick='saveSetup()'>Save Setup</button>
</div>
</div>

<script>
var questions=[];
function loadExisting(){
  try{var raw=localStorage.getItem('cueboard_engage_config');if(!raw){addQ();return}
    var c=JSON.parse(raw);
    if(c.wordCloudPrompt)document.getElementById('wc-prompt').value=c.wordCloudPrompt;
    if(c.ratingPrompt)document.getElementById('rate-prompt').value=c.ratingPrompt;
    if(c.pollQuestions&&c.pollQuestions.length){questions=c.pollQuestions;render()}else{addQ()}
  }catch(e){addQ()}
}
function addQ(){questions.push({type:'yn',text:'',options:[]});render()}
function delQ(i){questions.splice(i,1);render()}
function render(){
  var list=document.getElementById('q-list');var h='';
  questions.forEach(function(q,i){
    h+='<div class=""q-row""><div class=""num"">'+(i+1)+'.</div><div class=""body"">';
    h+='<select onchange=""setType('+i+',this.value)""><option value=""yn""'+(q.type==='yn'?' selected':'')+'>Yes / No</option><option value=""tf""'+(q.type==='tf'?' selected':'')+'>True / False</option><option value=""mc""'+(q.type==='mc'?' selected':'')+'>Multiple Choice</option></select>';
    h+='<textarea placeholder=""Type your question..."" oninput=""setText('+i+',this.value)"">'+escapeHtml(q.text||'')+'</textarea>';
    if(q.type==='mc')h+='<input type=""text"" placeholder=""Options, comma-separated (e.g. React, Vue, Angular)"" value=""'+escapeAttr((q.options||[]).join(', '))+'"" oninput=""setOpts('+i+',this.value)"" style=""font-size:12px""/>';
    h+='</div><button class=""del"" onclick=""delQ('+i+')"" title=""Remove"">&times;</button></div>';
  });
  list.innerHTML=h;
}
function escapeHtml(s){var d=document.createElement('div');d.textContent=s;return d.innerHTML}
function escapeAttr(s){return String(s).replace(/""/g,'&quot;')}
function setType(i,v){questions[i].type=v;if(v!=='mc')questions[i].options=[];render()}
function setText(i,v){questions[i].text=v}
function setOpts(i,v){questions[i].options=v.split(',').map(function(s){return s.trim()}).filter(function(s){return s})}
function saveSetup(){
  var config={
    wordCloudPrompt:document.getElementById('wc-prompt').value.trim(),
    ratingPrompt:document.getElementById('rate-prompt').value.trim(),
    pollQuestions:questions.filter(function(q){return q.text.trim()})
  };
  localStorage.setItem('cueboard_engage_config',JSON.stringify(config));
  document.getElementById('success').style.display='block';
  window.scrollTo({top:0,behavior:'smooth'});
  setTimeout(function(){document.getElementById('success').style.display='none'},5000);
}
function clearAll(){if(!confirm('Clear all questions and prompts?'))return;localStorage.removeItem('cueboard_engage_config');questions=[];document.getElementById('wc-prompt').value='';document.getElementById('rate-prompt').value='';addQ()}
loadExisting();
</script></body></html>";
        }
    }
}
