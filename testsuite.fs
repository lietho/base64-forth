: test { addr1 u1 addr2 u2 -- }
    addr1 u1 addr2 u2
    compare 0<> if
        addr1 u1 addr2 u2
        s" Expected: " type type s" , but was: " type type cr
    else 
        s" ok! " type
    endif ;
