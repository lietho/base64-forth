: test { addr1 u1 addr2 u2 -- }
    addr1 u1 addr2 u2
    compare 0<> if 
        addr1 u1 addr2 u2
        s" Expected: " type type s" , but was: " type type cr
    endif ;


\ taken from https://tools.ietf.org/html/rfc4648#section-10
s" " base64-encode s" " test
s" f"       base64-encode s" Zg=="      test
s" fo"      base64-encode s" Zm8="      test
s" foo"     base64-encode s" Zm9v"      test
s" foob"    base64-encode s" Zm9vYg=="  test
s" fooba"   base64-encode s" Zm9vYmE="  test
s" foobar"  base64-encode s" Zm9vYmFy"  test

s" Zg=="      base64-decode s" f"       test
s" Zm8="      base64-decode s" fo"      test
s" Zm9v"      base64-decode s" foo"     test
s" Zm9vYg=="  base64-decode s" foob"    test
s" Zm9vYmE="  base64-decode s" fooba"   test
s" Zm9vYmFy"  base64-decode s" foobar"  test

\ TODO: add test with abitrary binary data