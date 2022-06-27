
module Operation where

import Foreign.C.String
import Foreign.C.Types

foreign export ccall
    operation :: CInt -> IO CInt

operation :: CInt -> IO CInt
operation operand = return (factorial operand) 


foreign export ccall
    factorial :: CInt -> CInt

factorial :: CInt -> CInt
factorial 0 = 1
factorial num = if num > 0
                then num * (factorial (num-1))
                else -1
