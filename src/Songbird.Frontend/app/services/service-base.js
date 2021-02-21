import Service, { inject } from '@ember/service';
import fetch from 'fetch';
import { resolve, reject } from 'rsvp';

export default class ServiceBase extends Service {
  @inject router;

  /**
  * @param {RequestInfo} input
  * @param {RequestInit} [init]
  * @returns {Promise<Response>}
  */
  fetch(input, init) {
    return fetch(input, {
      redirect: 'manual',
      credentials: 'same-origin',
      ...init
    })
      .then(this.checkSuccess.bind(this));
  }

  /**
  * @param {Response} response
  * @returns {Promise<Response>}
  */
  checkSuccess(response) {
    if(response.type === 'opaqueredirect' || response.status === 401) {
      this.router.transitionTo('session-expired');
      throw new Error('Fetch request was redirected, probably due to session expiring.');
    }

    if(!response.ok) {
      return reject(response);
    }

    return resolve(response);
  }

  /**
  * @param {Response} response
  * @param {String} filename
  * @returns {Promise}
  */
  downloadResult(response, filename) {
    return response
      .blob()
      .then(blob => {
        let link = document.createElement('a');
        link.href = window.URL.createObjectURL(blob);
        link.download = filename;
        link.click();
      });
  }
}
