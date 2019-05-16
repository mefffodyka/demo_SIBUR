
from collections import OrderedDict

polka_1 = ['E20000196609011615205616', 'E2000019660901161490562A', 'E2000019660901161510561E']
polka_2 = ['E20000196609011614805626', 'E20000196609011614605632', 'E20000196609011614405636']
uid = 'user 1'
shkaf = polka_1+polka_2
timer_book = ['']

def chtenie_bd():

    first_file = open('C:/Users/Danil/Desktop/MainLine_PCReaderC#version_V2.18.0_20180711/SampleCode/SampleCode/out.txt','r')
    #line = first_file.read()
    first_line = [line.strip() for line in first_file]
    print (len(first_line))
    #sorted_lines = list(OrderedDict.fromkeys(first_line).keys())
    sorted_lines = list(set(first_line))
    print ("сортированный", sorted_lines)
    first_file.close()
    return sorted_lines

def chtenie_bd2():
    second_file = open('C:/Users/Danil/Desktop/MainLine_PCReaderC#version_V2.18.0_20180711/SampleCode/SampleCode/out2.txt','r')
    second_line = [line.strip() for line in second_file]
    print (len(second_line))
    sorted_lines2 = list(set(second_line))
    print ("сортированный", sorted_lines2)
    second_file.close()
    return sorted_lines2


def zabrali(sorted_lines,sorted_lines2):

    ss = list(set(sorted_lines) - set(sorted_lines2))

    a = shkaf.index(str(ss[0]))

    timer_book.append([ss[0],a,uid])
    
    print(ss,a,timer_book)
    temp = input()

def polozhili(sorted_lines,sorted_lines2):\

    ss = list(set(sorted_lines2) - set(sorted_lines))
    a = shkaf.index(str(ss[0]))

    timer_book.remove([ss[0],a,uid])
    
    print(ss,a,timer_book)
    temp = input()
if __name__ == '__main__':
    #vzyali
    a = chtenie_bd()
    b = chtenie_bd2()
    len_a = len(a)
    len_b = len(b)
    zabrali(a,b)
    nn=input()
    a = chtenie_bd()
    b = chtenie_bd2()
    polozhili(a,b)
#    if len_a>len_b:
#       zabrali(a,b)
#   elif len_a<len_b:
#       polozhili(a,b)



