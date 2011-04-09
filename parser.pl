print "prawdopodobienstwo krzyzowania;prawdopodobienstwo mutacji;minimum funkcji;uzyskane w pokoleniu\n";
while(<>){
  /- prawdopodobie.*stwo krzy.*owania:\s*([.-:digit:]+)/        ?  push(@p_krzyzowania, $1)
  : /- prawdopodobie.*stwo mutacji:\s*([.-:digit:]+)/           ?  push(@p_mutacji, $1) 
  : /min.*?f\(X\).*?([.-:digit:]+)/                      ?  push(@f_minimum, $1)
  : /uzyskane\s+w\s+(\d+)/                                      ?  push(@pokolenie, $1)
  : /Funkcja\s*(.+)$/ && !defined($funkcja)                  ?  chop($funkcja = $1)
  : /Skalowanie\s*(.+)$/ && !defined($skalowanie)             ?  chop($skalowanie = $1)
  : 1 ;
}

$minimum = 999;

while($p_k = pop(@p_krzyzowania)){
  $i++;
  $p_m = pop(@p_mutacji);
  $f_m = pop(@f_minimum);
  $p   = pop(@pokolenie);
  print "$p_k;$p_m;$f_m;$p\n";

  if ($f_m < $minimum){
    $minimum = $f_m;
    $p_kmin = $p_k;
    $p_min = $p;
    $p_pmin = $p_m;
  }
}

print STDERR "Wczytano $i eksperymentów\n";
print STDERR "funkcja $funkcja, skalowanie $skalowanie.\n";
print STDERR "Minimum $minimum dla p_krzyzowania $p_kmin i mutacji $p_pmin w pokoleniu $p_min.\n";