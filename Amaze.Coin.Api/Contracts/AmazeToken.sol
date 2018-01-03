pragma solidity ^0.4.18;

import 'github.com/OpenZeppelin/zeppelin-solidity/contracts/token/MintableToken.sol';

contract AmazeToken is MintableToken {

    event Burn(address from, uint256 value);

    string public name = 'Amaze Token';
    string public symbol = 'AMAZE';
    uint8 public decimals = 0;
    uint public INITIAL_SUPPLY = 50000000;

    function AmazeToken() public payable {
        totalSupply = INITIAL_SUPPLY;
        balances[msg.sender] = INITIAL_SUPPLY;
    }

    function burn(uint256 _value) public returns (bool success) {
        require(balanceOf(msg.sender) >= _value);
        balances[msg.sender] -= _value;
        totalSupply -= _value;
        Burn(msg.sender, _value);
        return true;
    }

    function burnFrom(address _from, uint256 _value) public onlyOwner returns (bool success) {
        require(balanceOf(_from) >= _value);
        require(_value <= allowance(_from, msg.sender)); 
        balances[_from] -= _value; 
        allowed[_from][msg.sender] -= _value;
        totalSupply -= _value; 
        Burn(_from, _value);
        return true;
    }
}