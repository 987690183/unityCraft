123456
import sys
from tkinter import *
from tkinter import ttk
from functools import partial

def accumulate():
    number1 = float(total.set(''))
    number2 = float(total.set(''))
        
def calculate(*args,value):
    #accumulate()
    #print (total.get())
    tempString = total.get()
    #avoid start with -, +
    if (total.get()==""):
            if (str(value)!="-" and str(value)!="+"):
                        total.set(total.get() + str(value))
    else:
        total.set(total.get() + str(value))
    if value == "=":
        #splitting the string up by + operater ['11', '2-9']
        print (tempString)
        stringArray=tempString.split('+')
        print (stringArray)
        
        minus=[]
        #splitting the string up by - operater , for example [['11'], ['2', '9']]
        for a in stringArray:
            minus.append(a.split('*'))
        print(minus)

        # we need a function from [['11'], ['2', '9']] to ['11', '-7']
        list1 = []
        for b in minus:
            total1=int(b[0])   
            for x in range(1,len(b)):
                total1=total1*int(b[x])
            list1.append(total1)
        print(list1)     
        #Added everything up in the array ['11', '-7']
        totalAmount=0
        for i in list1:
            totalAmount=totalAmount+i
        print (totalAmount)

        #print (int(tempString.split('+')[0]+int(tempString.split('+')[1])))
        total.set(totalAmount)
       
#top level root and mainframe of the GUI 
root = Tk()
root.title("Calculator")

mainframe = ttk.Frame(root, padding= "24 12 12 12")
mainframe.grid(column=10, row=10, sticky=(N, E, W, S))
mainframe.columnconfigure(1, weight=0)
mainframe.rowconfigure(1, weight=0)

total = StringVar()
value = StringVar()
accum = StringVar()
#newnum = StringVar(1)
current = StringVar()
number1=IntVar()
number2=IntVar()

# sets the initial total to 0
total.sfsdfet(str())
fsdfsdfsfdffffffffffffff
# the output/current number
ttk.Label(mainframe, textvariable=total).grid(column=1, row=1, sticky=(N, E, W, S))

# a button command uses partial to pass arguments to calculate function
ttk.Button(mainframe, text="1", command=partial(calculate,value = 1)).grid(column=1, row=2, sticky=(N, E, W, S))
ttk.Button(mainframe, text="2", command=partial(calculate,value = 2)).grid(column=2, row=2, sticky=(N, E, W, S))
ttk.Button(mainframe, text="3", command=partial(calculate,value = 3)).grid(column=3, row=2, sticky=(N, E, W, S))
ttk.Button(mainframe, text="4", command=partial(calculate,value = 4)).grid(column=1, row=3, sticky=(N, E, W, S))
ttk.Button(mainframe, text="5", command=partial(calculate,value = 5)).grid(column=2, row=3, sticky=(N, E, W, S))
ttk.Button(mainframe, text="6", command=partial(calculate,value=6)).grid(column=3, row=3, sticky=(N, E, W, S))
ttk.Button(mainframe, text="7", command=partial(calculate,value=7)).grid(column=1, row=4, sticky=(N, E, W, S))
ttk.Button(mainframe, text="8", command=partial(calculate,value=8)).grid(column=2, row=4, sticky=(N, E, W, S))
ttk.Button(mainframe, text="9", command=partial(calculate,value = 9)).grid(column=3, row=4, sticky=(N, E, W, S))
ttk.Button(mainframe, text="0", command=partial(calculate,value = 0)).grid(column=2, row=5, sticky=(N, E, W, S))
ttk.Button(mainframe, text=".", command=partial(calculate, value='.')).grid(column=1, row=5, sticky = (N,E,W,S))
ttk.Button(mainframe, text="C").grid(column=4, row=1, sticky = (N,E,W,S))
ttk.Button(mainframe, text="=", command=partial(calculate, value ='=')).grid(column=3, row=5)
ttk.Button(mainframe, text="+", command=partial(calculate, value ='+')).grid(column=4, row=2)
ttk.Button(mainframe, text="*", command=partial(calculate, value = '*')).grid(column=4, row=3)
ttk.Button(mainframe, text="/", command=partial(calculate, value = ':')).grid(column=4, row=4)
ttk.Button(mainframe, text="-", command=partial(calculate, value ='-')).grid(column=4, row=5)

# various things to tidy up the GUI
for child in mainframe.winfo_children(): child.grid_configure(padx=5, pady=5)
sdfsdf
root.bind('<Return>', calculate)

# start the GUI and loop waiting user input
root.mainloop()