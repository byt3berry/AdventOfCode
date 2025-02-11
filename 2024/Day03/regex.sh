RES=0
for MULT in $(pcre2grep -o 'mul\(\d{1,3},\d{1,3}\)|do\(\)|don.t\(\)' input.txt | pcre2grep -Mv 'don.t\(\)(\n.+$)*?\ndo\(\)' | pcre2grep -O '$1*$2' 'mul\((\d{1,3}),(\d{1,3})\)');
do
    RES=$(($RES+$(($MULT))));
done
echo $RES
