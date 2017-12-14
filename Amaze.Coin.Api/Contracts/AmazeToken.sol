pragma solidity ^0.4.4;

import 'zeppelin-solidity/contracts/token/StandardToken.sol';

contract AmazeToken is StandardToken {
    string public name = 'AmazeToken';
    string public symbol = 'AMZ';
    uint public decimals = 0;
    uint public INITIAL_SUPPLY = 1000000;

    function AmazeToken() {
        totalSupply = INITIAL_SUPPLY;
        balances[msg.sender] = INITIAL_SUPPLY;
    }
}