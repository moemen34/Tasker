import React, { Component } from 'react';

export class Home extends Component {
  static displayName = Home.name;

  render() {
    return (
      <div>
        <h1>Hello, world!</h1>
        <p>Welcome to your new single-page application, built with:</p>
        <ul>
          <li><a href='https://get.asp.net/'>ASP.NET Core</a> and <a href='https://msdn.microsoft.com/en-us/library/67ef8sbd.aspx'>C#</a> for cross-platform server-side code</li>
          <li><a href='https://facebook.github.io/react/'>React</a> for client-side code</li>
                <li><a href='http://getbootstrap.com/'>Bootstrap</a> and <a href='https://tailwindcss.com/'>TailwindCSS</a> for layout and styling</li>
        </ul>
            <p>Tasker is a webapp that allows organizations to assign tasks, view their progress, and mark as complete all following a Relationship based access control and authorization policy implemented with <a href='https://openfga.dev/'>OpenFGA</a>.</p>
        
      </div>
    );
  }
}
